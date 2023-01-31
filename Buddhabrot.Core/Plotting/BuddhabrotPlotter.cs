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
		/// Per-pixel orbit counts.
		/// </summary>
		private readonly int[] _orbitCounts;

		/// <summary>
		/// The total number of pixels in the resulting image.
		/// </summary>
		private readonly int _pixelCount;

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
			_pixelCount = _plot.Width * _plot.Height;
			_orbitCounts = new int [_pixelCount];
			_mandelbrotSetRegion = new(MinReal, MaxReal, MinImaginary, MaxImaginary);
			_mandelbrotSetRegion.MatchAspectRatio(_width, _height);
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", _parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		public override void Plot()
		{
			Log.Information("Plot starting.");
			_plot.Image = new ImageRgb(_width, _height);

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

					// Two or more threads could be incrementing the same point, so a synchronization method is necessary here.
					var index = pixelY * _width + pixelX;
					Interlocked.Increment(ref _orbitCounts[index]);
				}
			});

			var max = _orbitCounts.Max();
			Log.Information($"Plot complete, max orbit count: {max}.");

			// Use greyscale for now until a gradient palette is available.
			Parallel.For(0, _pixelCount, _parallelOptions, (i) =>
			{
				var index = i * ImageRgb.BytesPerPixel;
				_plot.Image.Data[index] = 
				_plot.Image.Data[index + 1] = 
				_plot.Image.Data[index + 2] = (byte)((double)_orbitCounts[i] / max * byte.MaxValue);
			});
			Log.Information("Image data written.");
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
					// Point has escaped to infinity.
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
	}
}
