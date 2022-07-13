using Buddhabrot.Core.Plotting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Buddhabrot.Test
{
	[TestClass]
	public class PlotterTests
	{
		class TestPlotter : Plotter
		{
			public TestPlotter(int width, int height, int maxIterations) : base(width, height, maxIterations)
			{

			}

			protected override void Plot()
			{
				throw new System.NotImplementedException();
			}
		}

		private readonly TestPlotter _plotter = new(1024, 768, 256);

		[TestMethod]
		public void PixelInBounds_WithNegativeX_ReturnsFalse()
		{
			var result = _plotter.PixelInBounds(-1, 384);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void PixelInBounds_WithOutOfBoundsX_ReturnsFalse()
		{
			var result = _plotter.PixelInBounds(2048, 384);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void PixelInBounds_WithNegativeY_ReturnsFalse()
		{
			var result = _plotter.PixelInBounds(512, -1);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void PixelInBounds_WithOutOfBoundsY_ReturnsFalse()
		{
			var result = _plotter.PixelInBounds(512, 1536);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void PixelInBounds_WithInBoundsXY_ReturnsTrue()
		{
			var result = _plotter.PixelInBounds(512, 384);

			Assert.IsTrue(result);
		}
	}
}
