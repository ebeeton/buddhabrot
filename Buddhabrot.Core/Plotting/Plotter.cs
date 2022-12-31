using Serilog;
using System.Numerics;

namespace Buddhabrot.Core.Plotting
{
	/// <summary>
	/// Common functionality for Mandelbrot and BuddhaBrot image plotting.
	/// </summary>
	public abstract class Plotter
	{
		/// <summary>
		/// The width of the image in pixels.
		/// </summary>
		protected readonly int _width;

		/// <summary>
		/// The height of the image in pixels.
		/// </summary>
		protected readonly int _height;

		/// <summary>
		/// "Because no complex number with a real or imaginary part greater
		/// than 2 can be part of the set, a common bailout is to escape when
		/// either coefficient exceeds 2."
		/// </summary>
		public const double Bailout = 2.0;

		/// <summary>
		/// Minimum real boundary of the Mandelbrot set.
		/// </summary>
		public const double MinReal = -2.0;

		/// <summary>
		/// Maximum real boundary of the Mandelbrot set.
		/// </summary>
		public const double MaxReal = 2.0;

		/// <summary>
		/// Minimum imaginary boundary of the Mandelbrot set.
		/// </summary>
		public const double MinImaginary = -2.0;

		/// <summary>
		/// Maximum imaginary boundary of the Mandelbrot set.
		/// </summary>
		public const double MaxImaginary = 2.0;

		/// <summary>
		/// Options for parallel processing.
		/// </summary>
		protected ParallelOptions _parallelOptions;

		/// <summary>
		/// Instantiates a plotter.
		/// </summary>
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		public Plotter(int width, int height)
		{
			_width = width;
			_height = height;

			// Plots can put all CPUs under full load. Leave one out so we don't starve everything else.
			var processors = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : 1;
			_parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = processors
			};
			Log.Information($"Plotter instantiated: {_width}x{_height} pixels. Using up to {processors} processors(s).");
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		public abstract void Plot();

		/// <summary>
		/// Determines if a point is in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point on the complex plane.</param>
		/// <param name="maxIterations">The maximum number of iterations to determine if the point is in the set or escapes to infinity.</param>
		/// <param name="iterations">The number of iterations for a point not in the set to escape to infinity.</param>
		/// <returns>True if a point is in the Mandelbrot set.</returns>
		public static bool IsInMandelbrotSet(Complex c, int maxIterations, ref int iterations)
		{
			var z = c;
			for (int i = 0; i < maxIterations; ++i)
			{
				if (double.Abs(z.Real) > Bailout || double.Abs(z.Imaginary) > Bailout)
				{
					// Not in the set.
					iterations = i;
					return false;
				}
				z = z * z + c;
			}

			// "Probably" in the set.
			return true;
		}

		/// <summary>
		/// Is a given pixel coordinate within the image bounds?
		/// </summary>
		/// <param name="pixelX">Horizontal coordinate.</param>
		/// <param name="pixelY">Vertical coordinate.</param>
		/// <returns></returns>
		public bool PixelInBounds(int pixelX, int pixelY)
		{
			return pixelX >= 0 && pixelX < _width && pixelY >= 0 && pixelY < _height;
		}
	}
}