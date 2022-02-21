namespace Buddhabrot.Core
{
	/// <summary>
	/// Performs linear scales.
	/// </summary>
	public static class LinearScale
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
		public static float Scale(float val, float minScaleFrom, float maxScaleFrom, float minScaleTo, float maxScaleTo)
		{
			// https://stackoverflow.com/a/5295202
			return (maxScaleTo - minScaleTo) * (val - minScaleFrom) / maxScaleFrom - minScaleFrom + minScaleTo;
		}
	}
}
