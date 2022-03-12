using Buddhabrot.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Buddhabrot.Test
{
	[TestClass]
	public class LinearScaleTests
	{
		[DataRow(5, 0, 10, 0, 100, 50)]
		[DataRow(50, 0, 100, 0, 1, 0.5f)]
		[DataRow(75, 0, 100, 0, 1, 0.75f)]
		[DataTestMethod]
		public void LinearScale_WithRanges_ScalesCorrectly(float val, float minScaleFrom, float maxScaleFrom, float minScaleTo, float maxScaleTo, float expected)
		{
			var actual = LinearScale.Scale(val, minScaleFrom, maxScaleFrom, minScaleTo, maxScaleTo);

			Assert.AreEqual(expected, actual);
		}
	}
}