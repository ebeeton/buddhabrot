using Buddhabrot.Core.Models;
using Buddhabrot.Core.Plotting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading.Tasks;

namespace Buddhabrot.Test.Core.Plotting
{
	[TestClass]
	public class MandelbrotPlotterTests
	{
		[TestMethod]
		public void PlotPng_WithParameters_ReturnsImageWithCorrectImageSize()
		{
			const int Height = 64, Width = 64, MaxIterations = 32;
			var plot = new Plot
			{
				Height = Height,
				Width = Width,
				PlotParams = JsonConvert.SerializeObject(new MandelbrotParameters
				{
					MaxIterations = MaxIterations
				}),
				PlotType = PlotType.Mandelbrot
			};

			var plotter = new MandelbrotPlotter(plot);

			plotter.Plot();
			using var image = Image.LoadPixelData<Rgb24>(plot.ImageData, plot.Width, plot.Height);

			Assert.AreEqual(Height, image.Height);
			Assert.AreEqual(Width, image.Width);
		}
	}
}
