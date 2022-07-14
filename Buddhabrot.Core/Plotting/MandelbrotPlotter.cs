using Buddhabrot.Core.Math;
using Buddhabrot.Core.Parameters;
using Serilog;
using System.Numerics;

namespace Buddhabrot.Core.Plotting
{
	/// <summary>
	/// A Mandelbrot image plotter.
	/// </summary>
	public class MandelbrotPlotter : Plotter
	{
		/// <summary>
		/// Instantiates a Mandelbrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		public MandelbrotPlotter(MandelbrotParameters parameters) : base(parameters.Width, parameters.Height)
		{
			MaxIterations = parameters.MaxIterations;
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", parameters);
		}

		/// <summary>
		/// Gets the maximum number of iterations to perform on each pixel.
		/// </summary>
		public int MaxIterations { get; protected set; }

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected override void Plot()
		{
			// Scale the vertical range so that the image doesn't squash or strech when
			// the image aspect ratio isn't 1:1.
			var scaleY = (double)Height / Width;
			var minY = scaleY * InitialMinY;
			var maxY = scaleY * InitialMaxY;

			// Plot each line in parallel.
			Parallel.For(0, Height, (y) =>
			{
				var imaginary = Linear.Scale(y, 0, Height, minY, maxY);

				for (int x = 0; x < BytesPerLine; x += RGBBytesPerPixel)
				{
					var real = Linear.Scale(x, 0, BytesPerLine, InitialMinX, InitialMaxX);

					int iterations = 0;
					if (IsInMandelbrotSet(new Complex(real, imaginary), MaxIterations, ref iterations))
					{
						// Leave points in the set black.
						continue;
					}

					// Grayscale plot based on how quickly the point escapes.
					var color = (byte)((double)iterations / MaxIterations * 255);
					var line = y * BytesPerLine;
					_imageBuffer[line + x] =
					_imageBuffer[line + x + 1] =
					_imageBuffer[line + x + 2] = color;
				}
			});
		}
	}
}
