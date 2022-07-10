using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Buddhabrot.Core
{
	/// <summary>
	/// Common functionality for Mandelbrot and BuddhaBrot image rendering.
	/// </summary>
	public abstract class Renderer
	{
		/// <summary>
		/// Instantiates a renderer.
		/// </summary>
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		/// <param name="maxIterations">Maximum number of iterations for each pixel.</param>
		public Renderer(int width, int height, int maxIterations)
		{
			Width = width;
			Height = height;
			MaxIterations = maxIterations;
			BytesPerLine = width * RGBBytesPerPixel;
			_imageData = new byte[Height * BytesPerLine];
		}

		/// <summary>
		/// The width of the image in pixels.
		/// </summary>
		protected readonly int Width;

		/// <summary>
		/// The height of the image in pixels.
		/// </summary>
		protected readonly int Height;

		/// <summary>
		/// The number of bytes in each line in the image.
		/// </summary>
		protected readonly int BytesPerLine;

		/// <summary>
		/// RGB 24 bits per-pixel image data.
		/// </summary>
		protected byte[] _imageData;

		/// <summary>
		/// The maximum number of iterations to process each point.
		/// </summary>
		protected readonly int MaxIterations;

		/// <summary>
		/// The number of bytes in a 24-bit RGB pixel.
		/// </summary>
		protected const int RGBBytesPerPixel = 3;

		/// <summary>
		/// <see cref="https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set">
		/// "Because no complex number with a real or imaginary part greater
		/// than 2 can be part of the set, a common bailout is to escape when
		/// either coefficient exceeds 2."</see>
		/// </summary>
		protected const double Bailout = 2.0;

		// Initial dimensions of the Mandelbrot set, more or less nicely centered.
		protected const double InitialMinX = -2;
		protected const double InitialMaxX = 0.47f;
		protected const double InitialMinY = -1.12f;
		protected const double InitialMaxY = 1.12f;

		/// <summary>
		/// Renders the Mandelbrot set to a PNG image.
		/// </summary>
		/// <returns>A task representing the work to render the image.</returns>
		public async Task<MemoryStream> RenderPng()
		{
			// Scale the vertical range so that the image doesn't squash or strech when
			// the image aspect ratio isn't 1:1.
			var scaleY = (double)Height / Width;
			var minY = scaleY * InitialMinY;
			var maxY = scaleY * InitialMaxY;

			// Render each line in parallel.
			Parallel.For(0, Height, (y) =>
			{
				var imaginary = LinearScale.Scale(y, 0, Height, minY, maxY);

				for (int x = 0; x < BytesPerLine; x += RGBBytesPerPixel)
				{
					var real = LinearScale.Scale(x, 0, BytesPerLine, InitialMinX, InitialMaxX);

					RenderPixel(x, y, new Complex(real, imaginary));
				}
			});

			using var image = Image.LoadPixelData<Rgb24>(_imageData, Width, Height);
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);

			return output;
		}

		/// <summary>
		/// Render a single pixel in the image.
		/// </summary>
		/// <param name="x">Horizontal pixel coordinate, left to right.</param>
		/// <param name="y">Vertical pixel coordinate, top to bottom.</param>
		/// <param name="c">Pixel scaled to the range in the Mandelbrot set.</param>
		protected abstract void RenderPixel(int x, int y, Complex c);

		/// <summary>
		/// Clears (reallocates) the image buffer.
		/// </summary>
		public void ClearImageBuffer()
		{
			_imageData = new byte[Height * BytesPerLine];
		}
	}
}