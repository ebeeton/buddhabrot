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
		/// How many random points on the complex plane are chosen to build the image.
		/// </summary>
		private const int SampleSize = 8192;

		/// <summary>
		/// Random points on the complex plane near - but not in - the Mandelbrot set.
		/// </summary>
		private readonly ConcurrentBag<Complex> _samplePoints;

		private static readonly Random _rand = new();

		/// <summary>
		/// Instantiates a Buddhabrot image plotter.
		/// </summary>
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		/// <param name="maxIterations"></param>
		public BuddhabrotPlotter(int width, int height, int maxIterations) : base(width, height, maxIterations)
		{
			_samplePoints = new ConcurrentBag<Complex>();
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected override void Plot()
		{
			// Generate a set of random points not in the Mandelbrot set.
			// We don't care about orbits yet.
			Parallel.For(0, SampleSize, _ =>
			{
				var point = RandomPointOnComplexPlane();
				if (IsInMandelbrotSet(point, ref _))
				{
					return;
				}

				_samplePoints.Add(point);
			});

			Log.Information($"Using sample size {SampleSize}. Sample points outside the Mandelbrot set: {_samplePoints.Count}.");

			// Iterate the sample set and plot their orbits.
			Parallel.ForEach(_samplePoints, p =>
			{
				int iterations = 0;
				var orbits = new Complex[MaxIterations];

				PlotOrbits(p, ref iterations, ref orbits);

				for (int i = 0; i < iterations; ++i)
				{
					var pixelX = (int)LinearScale.Scale(orbits[i].Real, InitialMinX, InitialMaxX, 0, Width);
					var pixelY = (int)LinearScale.Scale(orbits[i].Imaginary, InitialMinY, InitialMaxY, 0, Height);

					// TODO:: Test this before conversion from complex plane to pixels, save some time?
					if (!PixelInBounds(pixelX, pixelY))
					{
#if DEBUG
						Log.Warning($"Pixel out of bounds. X {pixelX} Y {pixelY} Width {Width} Height {Height}");
#endif
						continue;
					}

#if DEBUG
					Log.Debug($"Pixel in bounds. X {pixelX} Y {pixelY}");
#endif
					var index = pixelY * BytesPerLine + pixelX * RGBBytesPerPixel;
					lock (_imageData)
					{
						++_imageData[index];
						++_imageData[index + 1];
						++_imageData[index + 2];
					}
				}
			});
		}

		/// <summary>
		/// Generate a random point on the complex plane near the Mandelbrot set.
		/// </summary>
		/// <returns>A random point on the complex plane near the Mandelbrot set.</returns>
		protected static Complex RandomPointOnComplexPlane()
		{
			var real = _rand.NextDouble() * (InitialMaxX - InitialMinX) + InitialMinX;
			var imaginary = _rand.NextDouble() * (InitialMaxY - InitialMinY) + InitialMinY;
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
			for (int i = 0; i < MaxIterations; ++i)
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
	}
}
