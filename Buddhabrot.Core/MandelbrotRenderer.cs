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
