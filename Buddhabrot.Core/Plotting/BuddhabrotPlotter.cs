﻿using Buddhabrot.Core.Math;
using Buddhabrot.Core.Models;
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
		/// Whether to render the same pixel value to each channel.
		/// </summary>
		protected readonly bool _greyscale;

		/// <summary>
		/// Region on the complex plane containing the Mandelbrot set.
		/// </summary>
		protected readonly ComplexRegion _mandelbrotSetRegion;

		/// <summary>
		/// Instantiates a Buddhabrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		public BuddhabrotPlotter(BuddhabrotParameters parameters) : base(parameters.Width, parameters.Height)
		{
			_maxIterations = parameters.MaxIterations;
			_maxSampleIterations = parameters.MaxSampleIterations;
			_pixelsPerChannel = parameters.Width * parameters.Height;
			_sampleSize = (int)(parameters.SampleSize * _pixelsPerChannel);
			_channels = new int[RGBBytesPerPixel, _pixelsPerChannel];
			_passes = parameters.Passes;
			_greyscale = parameters.Grayscale;
			_mandelbrotSetRegion = new(InitialMinReal, InitialMaxReal, InitialMinImaginary, InitialMaxImaginary);
			_mandelbrotSetRegion.MatchAspectRatio(_width, _height);
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

			// Iterate the sample set and plot their orbits.
			Parallel.ForEach(samplePoints, p =>
			{
				int iterations = 0;
				var orbits = new Complex[_maxIterations];

				PlotOrbits(p, ref iterations, ref orbits);

				for (int i = 0; i < iterations; ++i)
				{
					var pixelX = (int)Linear.Scale(orbits[i].Real, _mandelbrotSetRegion.MinReal, _mandelbrotSetRegion.MaxReal, 0, _width);
					var pixelY = (int)Linear.Scale(orbits[i].Imaginary, _mandelbrotSetRegion.MinImaginary, _mandelbrotSetRegion.MaxImaginary, 0, _height);

					// Two or more threads could be incrementing the same pixel, so a synchronization method is necessary here.
					// Note that overflow is possible.
					if (!_greyscale)
					{
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
			var real = ThreadSafeRandom.NextDouble() * (InitialMaxReal - InitialMinReal) + InitialMinReal;
			var imaginary = ThreadSafeRandom.NextDouble() * (InitialMaxImaginary - InitialMinImaginary) + InitialMinImaginary;
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
				else if (_mandelbrotSetRegion.PointInRegion(z))
				{
					// Only save orbits if we know they're in the renderable region.
					orbits[i] = z;
				}
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
