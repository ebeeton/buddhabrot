using Buddhabrot.Core.Math;
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
		/// Instantiates a Buddhabrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		public BuddhabrotPlotter(BuddhabrotParameters parameters) : base(parameters.Width, parameters.Height)
		{
			MaxIterations = parameters.MaxIterations;
			MaxSampleIterations = parameters.MaxSampleIterations;
			SampleSize = parameters.SampleSize;
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", parameters);
		}

		/// <summary>
		/// Gets the maximum number of iterations to perform on each pixel.
		/// </summary>
		public int MaxIterations { get; protected set; }

		/// <summary>
		/// Gets/sets the maximum number of iterations for the intial sample set.
		/// </summary>
		public int MaxSampleIterations { get; protected set; }

		/// <summary>
		/// Gets how many random points on the complex plane are chosen to build the image.
		/// </summary>
		public int SampleSize { get; protected set; }

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected override void Plot()
		{
			// Generate a set of random points not in the Mandelbrot set.
			// We don't care about orbits yet.
			var samplePoints = new ConcurrentBag<Complex>();
			Parallel.For(0, SampleSize, _ =>
			{
				var point = RandomPointOnComplexPlane();
				if (IsInMandelbrotSet(point, MaxSampleIterations, ref _))
				{
					return;
				}

				samplePoints.Add(point);
			});

			Log.Information($"Using sample size {SampleSize}. Sample points outside the Mandelbrot set: {samplePoints.Count}.");

			// Iterate the sample set and plot their orbits.
			Parallel.ForEach(samplePoints, p =>
			{
				int iterations = 0;
				var orbits = new Complex[MaxIterations];

				PlotOrbits(p, ref iterations, ref orbits);

				for (int i = 0; i < iterations; ++i)
				{
					var pixelX = (int)Linear.Scale(orbits[i].Real, InitialMinX, InitialMaxX, 0, Width);
					var pixelY = (int)Linear.Scale(orbits[i].Imaginary, InitialMinY, InitialMaxY, 0, Height);

					// TODO:: Reject these before conversion from complex plane to pixels, save some cycles?
					if (!PixelInBounds(pixelX, pixelY))
					{
#if DEBUG
						Log.Warning($"Pixel out of bounds. X {pixelX} Y {pixelY} Width {Width} Height {Height}");
#endif
						continue;
					}

					var index = pixelY * BytesPerLine + pixelX * RGBBytesPerPixel;
					lock (_imageBuffer)
					{
						++_imageBuffer[index];
						++_imageBuffer[index + 1];
						++_imageBuffer[index + 2];
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
			var random = new Random();
			var real = random.NextDouble() * (InitialMaxX - InitialMinX) + InitialMinX;
			var imaginary = random.NextDouble() * (InitialMaxY - InitialMinY) + InitialMinY;
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
