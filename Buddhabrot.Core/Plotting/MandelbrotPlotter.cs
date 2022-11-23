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
		/// Gets the maximum number of iterations to perform on each pixel.
		/// </summary>
		protected readonly int _maxIterations;

		/// <summary>
		/// Instantiates a Mandelbrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		public MandelbrotPlotter(MandelbrotParameters parameters) : base(parameters.Width, parameters.Height)
		{
			_maxIterations = parameters.MaxIterations;
			Log.Information("Buddhabrot plotter instantiated: {@Parameters}", parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		protected override async Task Plot()
		{
			// Scale the vertical range so that the image doesn't squash or strech when
			// the image aspect ratio isn't 1:1.
			var aspectRatio = (double)_height / _width;
			var minImaginary = aspectRatio * InitialMinImaginary;
			var maxImaginary = aspectRatio * InitialMaxImaginary;

			// Plot each line in parallel.
			var task = Task.Run(() =>
			{
				Parallel.For(0, _height, (y) =>
				{
					var imaginary = Linear.Scale(y, 0, _height, minImaginary, maxImaginary);

					for (int x = 0; x < _bytesPerLine; x += RGBBytesPerPixel)
					{
						var real = Linear.Scale(x, 0, _bytesPerLine, InitialMinReal, InitialMaxReal);

						int iterations = 0;
						if (IsInMandelbrotSet(new Complex(real, imaginary), _maxIterations, ref iterations))
						{
							// Leave points in the set black.
							continue;
						}

						// Grayscale plot based on how quickly the point escapes.
						var color = (byte)((double)iterations / _maxIterations * 255);
						var line = y * _bytesPerLine;
						_imageBuffer[line + x] =
						_imageBuffer[line + x + 1] =
						_imageBuffer[line + x + 2] = color;
					}
				});
			});

			await task.WaitAsync(_plotTimeOut);
		}
	}
}
