using Buddhabrot.Core.Models;
using Buddhabrot.Core.Plotting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading.Tasks;

namespace Buddhabrot.Test.Core.Plotting
{
	[TestClass]
	public class MandelbrotPlotterTests
	{
		[TestMethod]
		public async Task PlotPng_WithParameters_ReturnsImageWithCorrectImageSize()
		{
			const int Height = 64, Width = 64, MaxIterations = 32;
			var parameters = new MandelbrotParameters
			{
				Height = Height,
				Width = Width,
				MaxIterations = MaxIterations
			};
			var plotter = new MandelbrotPlotter(parameters);

			var plot = await plotter.Plot();
			using var image = Image.LoadPixelData<Rgb24>(plot.ImageData, plot.Width, plot.Height);

			Assert.AreEqual(Height, image.Height);
			Assert.AreEqual(Width, image.Width);
		}
	}
}
