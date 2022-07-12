using Buddhabrot.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Buddhabrot.Test
{
	[TestClass]
	public class LinearScaleTests
	{
		/// <summary>
		/// "Close enough".
		/// </summary>
		const double Delta = 1e-6;

		[DataRow(5, 0, 10, 0, 100, 50)]
		[DataRow(50, 0, 100, 0, 1, 0.5)]
		[DataRow(75, 0, 100, 0, 1, 0.75)]
		[DataRow(0, -2, 0.47, 0, 3072, 2487.449392712551)]
		[DataTestMethod]
		public void LinearScale_WithRanges_ScalesCorrectly(double val, double minScaleFrom, double maxScaleFrom, double minScaleTo, double maxScaleTo, double expected)
		{
			var actual = LinearScale.Scale(val, minScaleFrom, maxScaleFrom, minScaleTo, maxScaleTo);

			Assert.AreEqual(expected, actual, Delta);
		}
	}
}