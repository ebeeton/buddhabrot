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
		/// <see cref="Plot"/>.
		/// </summary>
		protected Plot _plot;

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
		/// The total number of pixels per channel.
		/// </summary>
		protected readonly int _pixelsPerChannel;

		/// <summary>
		/// Region on the complex plane containing the Mandelbrot set.
		/// </summary>
		protected readonly ComplexRegion _mandelbrotSetRegion;

		/// <summary>
		/// <see cref="BuddhabrotParameters"/>.
		/// </summary>
		protected readonly BuddhabrotParameters _parameters;

		/// <summary>
		/// Real and imaginary components must be -2 to 2. This is a slight
		/// optimization when picking random points.
		/// </summary>
		public const double BailoutTimesTwo = 4.0;

		/// <summary>
		/// Instantiates a Buddhabrot image plotter.
		/// </summary>
		/// <param name="plot"><see cref="Plot"/>.</param>
		public BuddhabrotPlotter(Plot plot) : base(plot.Width, plot.Height)
		{
			_plot = plot;
			_parameters = plot.GetPlotParameters() as BuddhabrotParameters ??
				throw new ArgumentException($"Failed to get {nameof(BuddhabrotParameters)}.");
			_pixelsPerChannel = _plot.Width * _plot.Height;
			_channels = new int[RgbBytesPerPixel, _pixelsPerChannel];
			_mandelbrotSetRegion = new(InitialMinReal, InitialMaxReal, InitialMinImaginary, InitialMaxImaginary);
			_mandelbrotSetRegion.MatchAspectRatio(_width, _height);
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", _parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing the work to plot the image.</returns>
		public override async Task Plot()
		{
			_plot.PlotBeginUTC = DateTime.UtcNow;
			_plot.ImageData = _imageBuffer;

			// Plot each channel.
			Log.Information($"Beginning plot with {_parameters.SampleSize} sample points.");
			for (int i = 0; i < RgbBytesPerPixel; ++i)
			{
				await PlotChannel(i);
			}

			MergeFinalImage();

			_plot.PlotEndUTC = DateTime.UtcNow;
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
				Parallel.For(0, _parameters.SampleSize, _ =>
				{
					var point = RandomPointOnComplexPlane();
					if (IsInMandelbrotSet(point, _parameters.MaxSampleIterations, ref _))
					{
						return;
					}

					samplePoints.Add(point);
				});
			});
			await task.WaitAsync(_plotTimeOut);

			Log.Information($"Channel {channel} sample points outside the Mandelbrot set: {samplePoints.Count} ({((double)samplePoints.Count / _parameters.SampleSize * 100):0.#}%).");

			// Iterate the sample set and plot their orbits.
			Parallel.ForEach(samplePoints, p =>
			{
				var orbits = new List<Complex>();
				PlotOrbits(p, orbits);

				for (int i = 0; i < orbits.Count; ++i)
				{
					var pixelX = (int)Linear.Scale(orbits[i].Real, _mandelbrotSetRegion.MinReal, _mandelbrotSetRegion.MaxReal, 0, _width);
					var pixelY = (int)Linear.Scale(orbits[i].Imaginary, _mandelbrotSetRegion.MinImaginary, _mandelbrotSetRegion.MaxImaginary, 0, _height);

					// Two or more threads could be incrementing the same pixel, so a synchronization method is necessary here.
					var index = pixelY * _width + pixelX;
					Interlocked.Increment(ref _channels[channel, index]);
					// Clamp pixel value to byte.MaxValue because this is going to be treated as a byte when the final image is merged.
					Interlocked.CompareExchange(ref _channels[channel, index], byte.MaxValue, byte.MaxValue + 1);
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
			// [0,4] => [-2,2]
			var real = ThreadSafeRandom.NextDouble() * BailoutTimesTwo - Bailout;
			var imaginary = ThreadSafeRandom.NextDouble() * BailoutTimesTwo - Bailout;
			return new Complex(real, imaginary);
		}

		/// <summary>
		/// Plots the orbits of a point on the complex plane not in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point on the complex plane not in the Mandelbrot set.</param>
		/// <param name="orbits">The plottable orbits.</param>
		protected void PlotOrbits(Complex c, List<Complex> orbits)
		{
			var z = new Complex(0, 0);
			for (int i = 0; i < _parameters.MaxIterations; ++i)
			{
				if (double.Abs(z.Real) > Bailout || double.Abs(z.Imaginary) > Bailout)
				{
					// Orbit is outside the set.
					return;
				}
				else if (_mandelbrotSetRegion.PointInRegion(z))
				{
					// Only save orbits if we know they're in the renderable region.
					orbits.Add(z);
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
				var index = i * RgbBytesPerPixel;
				_imageBuffer[index] = (byte)_channels[(int)Channels.Red, i];
				_imageBuffer[index + 1] = (byte)_channels[(int)Channels.Green, i];
				_imageBuffer[index + 2] = (byte)_channels[(int)Channels.Blue, i];
			}
			Log.Debug("Final image merge complete.");
		}
	}
}
