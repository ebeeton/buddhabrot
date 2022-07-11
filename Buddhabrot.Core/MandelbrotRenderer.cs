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
		/// <param name="width">Width of the image in pixels.</param>
		/// <param name="height">Height of the image in pixels.</param>
		/// <param name="maxIterations"></param>
		public MandelbrotRenderer(int width, int height, int maxIterations) : base(width, height, maxIterations)
		{
			
		}

		/// <summary>
		/// Determines if a point is in the Mandelbrot set.
		/// </summary>
		/// <param name="c">A point represented as a complex number.</param>
		/// <param name="iterations">The number of iterations for points not in the set to escape to infinity.</param>
		/// <returns>True if a point is in the Mandelbrot set.</returns>
		public bool IsInMandelbrotSet(Complex c, ref int iterations)
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

		/// <summary>
		/// Render the image.
		/// </summary>
		protected override void Render()
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

					int iterations = 0;
					if (IsInMandelbrotSet(new Complex(real, imaginary), ref iterations))
					{
						// Leave points in the set black.
						continue;
					}

					// Grayscale plot based on how quickly the point escapes.
					var color = (byte)((double)iterations / MaxIterations * 255);
					var line = y * BytesPerLine;
					_imageData[line + x] =
					_imageData[line + x + 1] =
					_imageData[line + x + 2] = color;
				}
			});
		}
	}
}
