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
	public class BuddhabrotPlotterTests
	{
		[TestMethod]
		public async Task PlotPng_WithParameters_ReturnsImageWithCorrectImageSize()
		{
			const int Height = 64, Width = 64, MaxIterations = 32;
			var plot = new Plot
			{
				Height = Height,
				Width = Width,
				PlotParams = JsonConvert.SerializeObject(new BuddhabrotParameters
				{
					Height = Height,
					Width = Width,
					MaxIterations = MaxIterations,
					SampleSize = 0.1,
					MaxSampleIterations = MaxIterations,
				}),
				PlotType = PlotType.Buddhabrot
			};

			var plotter = new BuddhabrotPlotter(plot);

			await plotter.Plot();
			using var image = Image.LoadPixelData<Rgb24>(plot.ImageData, plot.Width, plot.Height);

			Assert.AreEqual(Height, image.Height);
			Assert.AreEqual(Width, image.Width);
		}
	}
}
