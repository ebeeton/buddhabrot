using Buddhabrot.Core.Models;
using Buddhabrot.Core.Plotting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Buddhabrot.Test.Core.Plotting
{
	[TestClass]
	public class PlotterFactoryTests
	{
		[TestMethod]
		public void GetPlotter_WithMandelbrotType_ReturnsMandelbrotPlotter()
		{
			var plot = new Plot()
			{
				PlotType = PlotType.Mandelbrot,
				PlotParams = JsonConvert.SerializeObject(new MandelbrotParameters())
			};

			var plotter = PlotterFactory.GetPlotter(plot);

			Assert.IsNotNull(plotter as MandelbrotPlotter);
		}

		[TestMethod]
		public void GetPlotter_WithBuddhabrotType_ReturnsBuddhabrotPlotter()
		{
			var plot = new Plot()
			{
				PlotType = PlotType.Buddhabrot,
				PlotParams = JsonConvert.SerializeObject(new BuddhabrotParameters())
			};

			var plotter = PlotterFactory.GetPlotter(plot);

			Assert.IsNotNull(plotter as BuddhabrotPlotter);
		}
	}
}
