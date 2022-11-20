using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace Buddhabrot.Core.Plotting
{
	/// <summary>
	/// Common functionality for Mandelbrot and BuddhaBrot image plotting.
	/// </summary>
	public abstract class Plotter
	{
		/// <summary>
		/// 24 bits per pixel image buffer.
		/// </summary>
		protected byte[] _imageBuffer;

		/// <summary>
		/// The number of bytes in a 24 bit RGB pixel.
		/// </summary>
		protected const int RGBBytesPerPixel = 3;

		/// <summary>
		/// The width of the image in pixels.
		/// </summary>
		protected readonly int _width;

		/// <summary>
		/// The height of the image in pixels.
		/// </summary>
		protected readonly int _height;

		/// <summary>
		/// The number of bytes in each line in the image.
		/// </summary>
		protected readonly int _bytesPerLine;

		/// <summary>
		/// <see cref="https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set">
		/// "Because no complex number with a real or imaginary part greater
		/// than 2 can be part of the set, a common bailout is to escape when
		/// either coefficient exceeds 2."</see>
		/// </summary>
		protected const double Bailout = 2.0;

		// Initial dimensions of the Mandelbrot set, more or less nicely centered.
		protected const double InitialMinX = -2;
		protected const double InitialMaxX = 0.47;
		protected const double InitialMinY = -1.12;
		protected const double InitialMaxY = 1.12;

		/// <summary>
		/// The duration which, when exceeded, will cause the plot to time out.
		/// </summary>
		protected static readonly TimeSpan _plotTimeOut = new(hours: 1, minutes: 0, seconds: 0);

		protected readonly float _rotateDegrees;

		/// <summary>
		/// Instantiates a plotter.
		/// </summary>
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		/// <param name="rotateDegrees">Degrees to rotate the final image.</param>
		public Plotter(int width, int height, float rotateDegrees = 0.0f)
		{
			_width = width;
			_height = height;
			_bytesPerLine = width * RGBBytesPerPixel;
			_imageBuffer = new byte[_height * _bytesPerLine];
			_rotateDegrees = rotateDegrees;
			Log.Information($"Plotter instantiated: {_width}x{_height} pixels.");
		}

		/// <summary>
		/// Plot the Mandelbrot set to a PNG image.
		/// </summary>
		/// <returns>A task representing the work to render the image.</returns>
		public async Task<MemoryStream> PlotPng()
		{
			await Plot();

			using var image = Image.LoadPixelData<Rgb24>(_imageBuffer, _width, _height);
			if (_rotateDegrees != 0.0f)
			{
				image.Mutate(i => i.Rotate(_rotateDegrees));
			}
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);

			return output;
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected abstract Task Plot();

		/// <summary>
		/// Clears (reallocates) the image buffer.
		/// </summary>
		public void ClearImageBuffer()
		{
			_imageBuffer = new byte[_height * _bytesPerLine];
		}

		/// <summary>
		/// Determines if a point is in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point on the complex plane.</param>
		/// <param name="maxIterations">The number of iterations for points not in the set to escape to infinity.</param>
		/// <param name="iterations">The number of iterations for points not in the set to escape to infinity.</param>
		/// <returns>True if a point is in the Mandelbrot set.</returns>
		public static bool IsInMandelbrotSet(Complex c, int maxIterations, ref int iterations)
		{
			var z = new Complex(0, 0);
			for (int i = 0; i < maxIterations; ++i)
			{
				if (z.Magnitude > Bailout)
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