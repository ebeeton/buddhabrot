using Buddhabrot.Core.Math;
using Buddhabrot.Core.Models;
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
		/// <see cref="Plot"/>.
		/// </summary>
		private Plot _plot;

		/// <summary>
		/// <see cref="MandelbrotParameters"/>.
		/// </summary>
		private readonly MandelbrotParameters _parameters;

		/// <summary>
		/// Instantiates a Mandelbrot image plotter.
		/// </summary>
		/// <param name="plot">Parameters used to plot the image.</param>
		public MandelbrotPlotter(Plot plot) : base(plot.Width, plot.Height)
		{
			_plot = plot;
			_parameters = plot.GetPlotParameters() as MandelbrotParameters ??
				throw new ArgumentException($"Failed to get {nameof(MandelbrotParameters)}.");
			Log.Information("Mandelbrot plotter instantiated: {@Parameters}", _parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		public override void Plot()
		{
			_plot.ImageData = _imageBuffer;

			// Scale the vertical range so that the image doesn't squash or strech when
			// the image aspect ratio isn't 1:1.
			var aspectRatio = (double)_height / _width;
			var minImaginary = aspectRatio * MinImaginary;
			var maxImaginary = aspectRatio * MaxImaginary;

			// Plot each line in parallel.
			Parallel.For(0, _height, _parallelOptions, (y) =>
			{
				var imaginary = Linear.Scale(y, 0, _height, minImaginary, maxImaginary);

				for (int x = 0; x < _bytesPerLine; x += RgbBytesPerPixel)
				{
					var real = Linear.Scale(x, 0, _bytesPerLine, MinReal, MaxReal);

					int iterations = 0;
					if (IsInMandelbrotSet(new Complex(real, imaginary), _parameters.MaxIterations, ref iterations))
					{
						// Leave points in the set black.
						continue;
					}

					// Grayscale plot based on how quickly the point escapes.
					var color = (byte)((double)iterations / _parameters.MaxIterations * 255);
					var line = y * _bytesPerLine;
					_imageBuffer[line + x] =
					_imageBuffer[line + x + 1] =
					_imageBuffer[line + x + 2] = color;
				}
			});
		}
	}
}
