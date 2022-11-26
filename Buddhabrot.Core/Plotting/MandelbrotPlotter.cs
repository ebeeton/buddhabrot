using Buddhabrot.Core.Math;
using Buddhabrot.Core.Models;
using Newtonsoft.Json;
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
		/// <see cref="MandelbrotParameters"/>.
		/// </summary>
		protected readonly MandelbrotParameters _parameters;

		/// <summary>
		/// Instantiates a Mandelbrot image plotter.
		/// </summary>
		/// <param name="parameters">Parameters used to plot the image.</param>
		public MandelbrotPlotter(MandelbrotParameters parameters) : base(parameters.Width, parameters.Height)
		{
			_parameters = parameters;
			Log.Information("Mandelbrot plotter instantiated: {@Parameters}", parameters);
		}

		/// <summary>
		/// Plot the image.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing the work to plot the image.</returns>
		public override async Task<Plot> Plot()
		{
			var plot = new Plot
			{
				PlotBeginUTC = DateTime.UtcNow,
				ImageData = _imageBuffer,
				Width = _width,
				Height = _height,
				ParametersJson = JsonConvert.SerializeObject(_parameters),
				Type = Models.Plot.PlotType.Mandelbrot
			};

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
			});

			await task.WaitAsync(_plotTimeOut);
			plot.PlotEndUTC = DateTime.UtcNow;
			return plot;
		}
	}
}
