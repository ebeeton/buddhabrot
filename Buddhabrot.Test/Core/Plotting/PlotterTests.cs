using Buddhabrot.Core.Plotting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Buddhabrot.Test.Core.Plotting
{
    [TestClass]
    public class PlotterTests
    {
        const int MaxIterations = 1000;

        class TestPlotter : Plotter
        {
            public TestPlotter(int width, int height) : base(width, height)
            {

            }

            public override void Plot()
            {
                
            }
        }

        private readonly TestPlotter _plotter = new(1024, 768);

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

        [TestMethod]
        public void IsInMandelbrotSet_WithPointInSet_ReturnsTrue()
        {
            int iterations = 0;
            var c = new Complex(0, 0);

            var isInSet = Plotter.IsInMandelbrotSet(c, MaxIterations, ref iterations);

            Assert.IsTrue(isInSet);
            Assert.AreEqual(0, iterations);
		}

		[TestMethod]
		public void IsInMandelbrotSet_WithPointNotInSet_ReturnsFalse()
		{
			int iterations = 0;
			var c = new Complex(1, 1);

			var isInSet = Plotter.IsInMandelbrotSet(c, MaxIterations, ref iterations);

			Assert.IsFalse(isInSet);
			Assert.AreEqual(1, iterations);
		}
	}
}
