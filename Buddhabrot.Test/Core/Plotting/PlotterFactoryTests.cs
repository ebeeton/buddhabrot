using Buddhabrot.Core.Models;
using Buddhabrot.Core.Plotting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Buddhabrot.Test.Core.Plotting
{
	[TestClass]
	public class PlotterFactoryTests
	{
		[TestMethod]
		public void GetPlotter_WithMandelbrotType_ReturnsMandelbrotPlotter()
		{
			var type = PlotType.Mandelbrot;
			var parameters = "{\"Width\":1024,\"Height\":768,\"MaxIterations\":128}";

			var plotter = PlotterFactory.GetPlotter(type, parameters);

			Assert.IsNotNull(plotter as MandelbrotPlotter);
		}

		[TestMethod]
		public void GetPlotter_WithBuddhabrotType_ReturnsMandelbrotPlotter()
		{
			var type = PlotType.Buddhabrot;
			var parameters = "{\"Width\":1024,\"Height\":768,\"MaxIterations\":16384,\"MaxSampleIterations\":2048,\"SampleSize\":0.01,\"Passes\":32,\"Grayscale\":false}";

			var plotter = PlotterFactory.GetPlotter(type, parameters);

			Assert.IsNotNull(plotter as BuddhabrotPlotter);
		}
	}
}
