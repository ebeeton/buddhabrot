using Buddhabrot.Core.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Buddhabrot.Test.Core.Math
{
	[TestClass]
	public class ComplexRegionTests
	{
		[TestMethod]
		public void PointInRegion_WithPointInsideRegion_ReturnsTrue()
		{
			var region = new ComplexRegion(-2.4d, 0.2d, -2.9d, 3.4d);
			var point = new Complex(-2.2d, 2.6d);

			var result = region.PointInRegion(point);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void PointInRegion_WithPointOutsideRegion_ReturnsFalse()
		{
			var region = new ComplexRegion(-2.4d, 0.2d, -2.9d, 3.4d);
			var point = new Complex(-2.9d, 2.6d);

			var result = region.PointInRegion(point);

			Assert.IsFalse(result);
		}
	}
}
