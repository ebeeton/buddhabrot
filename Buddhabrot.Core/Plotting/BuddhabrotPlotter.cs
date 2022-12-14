using Buddhabrot.Core.Math;
using Buddhabrot.Core.Models;
using Serilog;
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
		private readonly Plot _plot;

		/// <summary>
		/// Symbolic constants for RGB channel colors.
		/// </summary>
		protected enum Channels
		{
			Red = 0,
			Green,
			Blue,
			All
		};

		/// <summary>
		/// RGB channels of width * height.
		/// </summary>
		private readonly int[][] _channels;

		/// <summary>
		/// The total number of pixels per channel.
		/// </summary>
		private readonly int _pixelsPerChannel;

		/// <summary>
		/// Region on the complex plane containing the Mandelbrot set.
		/// </summary>
		private readonly ComplexRegion _mandelbrotSetRegion;

		/// <summary>
		/// <see cref="BuddhabrotParameters"/>.
		/// </summary>
		private readonly BuddhabrotParameters _parameters;

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
			_channels = new int[RgbBytesPerPixel][]
			{
				new int [_pixelsPerChannel],
				new int [_pixelsPerChannel],
				new int [_pixelsPerChannel]
			};
			_mandelbrotSetRegion = new(MinReal, MaxReal, MinImaginary, MaxImaginary);
			_mandelbrotSetRegion.MatchAspectRatio(_width, _height);
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", _parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		public override void Plot()
		{
			_plot.ImageData = _imageBuffer;

			// Plot each channel.
			Log.Information($"Beginning plot with {_parameters.SampleSize} sample points.");
			for (int i = 0; i < RgbBytesPerPixel; ++i)
			{
				PlotChannel(i);
			}

			MergeFinalImage();
		}

		/// <summary>
		/// Plot a Buddhabrot image to one of the RGB image channels.
		/// </summary>
		/// <param name="channel">Index of channel to plot to.</param>
		protected void PlotChannel(int channel)
		{
			Log.Information($"Channel {channel} plot started.");

			// Iterate sample points not in the Mandelbrot set and plot their orbits.
			Parallel.For(0, _parameters.SampleSize, _parallelOptions, _ =>
			{
				Complex point;
				do
				{
					point = RandomPointOnComplexPlane();
				} while (IsInMandelbrotSet(point, _parameters.MaxSampleIterations, ref _));

				var orbits = new List<Complex>();
				PlotOrbits(point, orbits);

				for (int i = 0; i < orbits.Count; ++i)
				{
					var pixelX = (int)Linear.Scale(orbits[i].Real, _mandelbrotSetRegion.MinReal, _mandelbrotSetRegion.MaxReal, 0, _width);
					var pixelY = (int)Linear.Scale(orbits[i].Imaginary, _mandelbrotSetRegion.MinImaginary, _mandelbrotSetRegion.MaxImaginary, 0, _height);

					// Two or more threads could be incrementing the same pixel, so a synchronization method is necessary here.
					var index = pixelY * _width + pixelX;
					Interlocked.Increment(ref _channels[channel][index]);
					// Clamp pixel value to byte.MaxValue because this is going to be treated as a byte when the final image is merged.
					Interlocked.CompareExchange(ref _channels[channel][index], byte.MaxValue, byte.MaxValue + 1);
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
			var z = c;
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

			// Get the scale factor to go from hit counts to one byte per channel.
			var scaleFactors = new double[(int)Channels.All];
			Parallel.For(0, (int)Channels.All, _parallelOptions, (i) => scaleFactors[i] = _channels[i].Max());
			Log.Debug("Scale factors: {@scaleFactors}", scaleFactors);

			Parallel.For(0, _pixelsPerChannel, _parallelOptions, (i) =>
			{
				var index = i * RgbBytesPerPixel;
				_imageBuffer[index] = (byte)(_channels[(int)Channels.Red][i] / scaleFactors[(int)Channels.Red] * byte.MaxValue);
				_imageBuffer[index + 1] = (byte)(_channels[(int)Channels.Green][i] / scaleFactors[(int)Channels.Blue] * byte.MaxValue);
				_imageBuffer[index + 2] = (byte)(_channels[(int)Channels.Blue][i] / scaleFactors[(int)Channels.Green] * byte.MaxValue);
			});
			Log.Debug("Final image merge complete.");
		}
	}
}
