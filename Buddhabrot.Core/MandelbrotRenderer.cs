using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Buddhabrot.Core
{
	/// <summary>
	/// A Mandelbrot image renderer.
	/// </summary>
	public class MandelbrotRenderer : Renderer
	{
		/// <summary>
		/// Instantiates a Mandelbrot image renderer.
		/// </summary>
		/// <param name="maxIterations"></param>
		public MandelbrotRenderer(int maxIterations) : base(maxIterations)
		{
			
		}

		/// <summary>
		/// # https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set
		/// "Because no complex number with a real or imaginary part greater
		/// than 2 can be part of the set, a common bailout is to escape when
		/// either coefficient exceeds 2."
		/// </summary>
		private const double Bailout = 2.0;

		// Initial dimensions of the Mandelbrot set, more or less nicely centered.
		private const float InitialMinX = -2;
		private const float InitialMaxX = 0.47f;
		private const float InitialMinY = -1.12f;
		private const float InitialMaxY = 1.12f;

		/// <summary>
		/// Renders the Mandelbrot set to a PNG image.
		/// </summary>
		/// <returns>A task representing the work to render the image.</returns>
		public async Task<MemoryStream> RenderPng(int width, int height)
		{
			// Scale the vertical range so that the image doesn't squash or strech when
			// the image aspect ratio isn't 1:1.
			var scaleY = (float)height / width;
			var minY = scaleY * InitialMinY;
			var maxY = scaleY * InitialMaxY;

			var bytesPerLine = width * RGBBytesPerPixel;

			var data = new byte[height * bytesPerLine];

			Parallel.For(0, height, (y) =>
			{
				var imaginary = LinearScale.Scale(y, 0, height, minY, maxY);

				for (int x = 0; x < bytesPerLine; x += 3)
				{
					var real = LinearScale.Scale(x, 0, bytesPerLine, InitialMinX, InitialMaxX);

					int iterations = 0;
					if (IsInMandelbrotSet(new Complex(real, imaginary), ref iterations))
					{
						// Leave points in the set black.
						continue;
					}

					// Grayscale plot based on how quickly the point escapes.
					var color = (byte)((double)iterations / MaxIterations * 255);
					var line = y * bytesPerLine;
					data[line + x] =
					data[line + x + 1] =
					data[line + x + 2] = color;
				}
			});

			using var image = Image.LoadPixelData<Rgb24>(data, width, height);
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);

			return output;
		}

		private bool IsInMandelbrotSet(Complex c, ref int iterations)
		{
			var z = new Complex(0, 0);
			for (int i = 0; i < MaxIterations; ++i)
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
	}
}
