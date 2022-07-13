namespace Buddhabrot.Core.Math
{
	/// <summary>
	/// Performs linear scales.
	/// </summary>
	public static class Linear
	{
		/// <summary>
		/// Linearly scales a value in one range to another.
		/// </summary>
		/// <param name="val">Value to scale.</param>
		/// <param name="minScaleFrom">Minimum value of the range scaling from.</param>
		/// <param name="maxScaleFrom">Maximum value of the range scaling from.</param>
		/// <param name="minScaleTo">Minimum value of the range scaling to.</param>
		/// <param name="maxScaleTo">Maximum value of the range scaling to.</param>
		/// <returns>Linearly scaled value.</returns>
		public static double Scale(double val, double minScaleFrom, double maxScaleFrom, double minScaleTo, double maxScaleTo)
		{
			return (val - minScaleFrom) / (maxScaleFrom - minScaleFrom) * (maxScaleTo - minScaleTo) + minScaleTo;
		}
	}
}
