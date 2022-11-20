﻿using Buddhabrot.Core.Math;
using Buddhabrot.Core.Parameters;
using Serilog;
using System.Collections.Concurrent;
using System.Numerics;

namespace Buddhabrot.Core.Plotting
{
	/// <summary>
	/// A Buddhabrot image plotter.
	/// </summary>
	public class BuddhabrotPlotter : Plotter
	{
		/// <summary>
		/// Symbolic constants for RGB channel colors.
		/// </summary>
		protected enum Channels
		{
			Red = 0,
			Green,
			Blue,
		};

		/// <summary>
		/// RGB channels of width * height.
		/// </summary>
		protected int[,] _channels;

		/// <summary>
		/// The maximum number of iterations to perform on each pixel.
		/// </summary>
		protected readonly int _maxIterations;

		/// <summary>
		/// The maximum number of iterations for the intial sample set.
		/// </summary>
		protected readonly int _maxSampleIterations;

		/// <summary>
		/// How many random points on the complex plane are chosen to build the image.
		/// </summary>
		protected readonly int _sampleSize;

		/// <summary>
		/// The total number of pixels per channel.
		/// </summary>
		protected readonly int _pixelsPerChannel;

		/// <summary>
		/// Number of passes to build the image.
		/// </summary>
		protected readonly int _passes;

		/// <summary>
		/// Degrees to rotate the final image to produce the familiar Buddhabrot shape.
		/// </summary>
		protected const float RotateDegrees = 90;

		/// <summary>
		/// Whether to render the same pixel value to each channel.
		/// </summary>
		protected readonly bool _greyscale;

		/// <summary>
		/// Instantiates a Buddhabrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		/// <remarks>
		/// Note that width and height are intentionally swapped here because
		/// the final image will be rotated 90 clockwise.
		/// </remarks>
		public BuddhabrotPlotter(BuddhabrotParameters parameters) : base(parameters.Height, parameters.Width, RotateDegrees)
		{
			_maxIterations = parameters.MaxIterations;
			_maxSampleIterations = parameters.MaxSampleIterations;
			_pixelsPerChannel = parameters.Width * parameters.Height;
			_sampleSize = (int)(parameters.SampleSize * _pixelsPerChannel);
			_channels = new int[RGBBytesPerPixel, _pixelsPerChannel];
			_passes = parameters.Passes;
			_greyscale = parameters.Grayscale;
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected override async Task Plot()
		{
			// Plot each channel.
			Log.Information($"Beginning plot with {_sampleSize} sample points.");
			for (int i = 0; i < _passes; i++)
			{
				Log.Debug($"Pass {i + 1} started.");
				for (int j = 0; j < RGBBytesPerPixel; ++j)
				{
					await PlotChannel(j);
				}
				Log.Debug($"Pass {i + 1} complete.");
			}

			if (!_greyscale)
			{
				MergeFinalImage();
			}
		}

		/// <summary>
		/// Plot a Buddhabrot image to one of the RGB image channels.
		/// </summary>
		/// <param name="channel">Index of channel to plot to.</param>
		protected async Task PlotChannel(int channel)
		{
			// Generate a set of random points not in the Mandelbrot set.
			// We don't care about orbits yet.
			Log.Information($"Channel {channel} plot started.");
			var samplePoints = new ConcurrentBag<Complex>();
			var task = Task.Run(() =>
			{
				Parallel.For(0, _sampleSize, _ =>
				{
					var point = RandomPointOnComplexPlane();
					if (IsInMandelbrotSet(point, _maxSampleIterations, ref _))
					{
						return;
					}

					samplePoints.Add(point);
				});
			});
			await task.WaitAsync(_plotTimeOut);

			Log.Information($"Channel {channel} sample points outside the Mandelbrot set: {samplePoints.Count} ({((double)samplePoints.Count / _sampleSize * 100):0.#}%).");

			// Scale the vertical range so that the image doesn't squash or strech when
			// the image aspect ratio isn't 1:1.
			var scaleX = (double)_width / _height;
			var minX = scaleX * InitialMinX;
			var maxX = scaleX * InitialMaxX;

			// Iterate the sample set and plot their orbits.
			Parallel.ForEach(samplePoints, p =>
			{
				int iterations = 0;
				var orbits = new Complex[_maxIterations];

				PlotOrbits(p, ref iterations, ref orbits);

				for (int i = 0; i < iterations; ++i)
				{
					var pixelX = (int)Linear.Scale(orbits[i].Real, minX, maxX, 0, _width);
					var pixelY = (int)Linear.Scale(orbits[i].Imaginary, InitialMinY, InitialMaxY, 0, _height);

					// TODO:: Reject these before conversion from complex plane to pixels, save some cycles?
					if (!PixelInBounds(pixelX, pixelY))
					{
						continue;
					}
					else if (!_greyscale)
					{
						// Two or more threads could be incrementing the same pixel, so a synchronization method is necessary here.
						// Note that overflow is possible.
						var index = pixelY * _width + pixelX;
						Interlocked.Increment(ref _channels[channel, index]);
					}
					else
					{
						var index = pixelY * _bytesPerLine + pixelX * RGBBytesPerPixel;
						lock (_imageBuffer)
						{
							++_imageBuffer[index];
							++_imageBuffer[index + 1];
							++_imageBuffer[index + 2];
						}
					}
				}
			});

			Log.Information($"Channel {channel} plot complete.");
		}

		/// <summary>
		/// Generate a random point on the complex plane near the Mandelbrot set.
		/// </summary>
		/// <returns>A random point on the complex plane near the Mandelbrot set.</returns>
		protected static Complex RandomPointOnComplexPlane()
		{
			var real = ThreadSafeRandom.NextDouble() * (InitialMaxX - InitialMinX) + InitialMinX;
			var imaginary = ThreadSafeRandom.NextDouble() * (InitialMaxY - InitialMinY) + InitialMinY;
			return new Complex(real, imaginary);
		}

		/// <summary>
		/// Plots the orbits of a point on the complex plane not in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point on the complex plane not in the Mandelbrot set.</param>
		/// <param name="iterations">The number of iterations to escape to infinity.</param>
		/// <param name="orbits">An array of size MaxIterations.</param>
		protected void PlotOrbits(Complex c, ref int iterations, ref Complex[] orbits)
		{
			var z = new Complex(0, 0);
			for (int i = 0; i < _maxIterations; ++i)
			{
				if (z.Magnitude > Bailout)
				{
					// Not in the set.
					iterations = i;
					return;
				}

				orbits[i] = z;
				z = z * z + c;
			}
		}

		/// <summary>
		/// Combine the separate RGB channels into the final image output data.
		/// </summary>
		protected void MergeFinalImage()
		{
			Log.Debug("Final image merge started.");
			for (int i = 0; i < _pixelsPerChannel; ++i)
			{
				var index = i * RGBBytesPerPixel;
				_imageBuffer[index] = (byte)_channels[(int)Channels.Red, i];
				_imageBuffer[index + 1] = (byte)_channels[(int)Channels.Green, i];
				_imageBuffer[index + 2] = (byte)_channels[(int)Channels.Blue, i];
			}
			Log.Debug("Final image merge complete.");
		}
	}
}
