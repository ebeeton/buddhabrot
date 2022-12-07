using Buddhabrot.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Buddhabrot.Test.API.Models
{
	[TestClass]
	public class PlotTests
	{
		[TestMethod]
		public void GetPlotParameters_WithValidJson_ReturnsMandelbrotParameters()
		{
			var plot = new Plot
			{
				PlotParams = @"{""Width"":1024,""Height"":768,""MaxIterations"":128}",
				PlotType = PlotType.Mandelbrot,
			};

			var parameters = plot.GetPlotParameters() as MandelbrotParameters;

			Assert.IsNotNull(parameters);

		}

		[TestMethod]
		public void GetPlotParameters_WithValidJson_ReturnsBuddhabrotParameters()
		{
			var plot = new Plot
			{
				PlotParams = @"{""Width"":1024,""Height"":768,""MaxIterations"":16384,""MaxSampleIterations"":2048,""SampleSize"":0.01,""Passes"":32}",
				PlotType = PlotType.Buddhabrot
			};

			var parameters = plot.GetPlotParameters() as BuddhabrotParameters;

			Assert.IsNotNull(parameters);
		}
	}
}
