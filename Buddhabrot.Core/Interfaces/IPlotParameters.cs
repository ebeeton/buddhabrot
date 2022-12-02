namespace Buddhabrot.Core.Interfaces
{
	/// <summary>
	/// Parameters used to generate a plot.
	/// </summary>
	public interface IPlotParameters
	{
		/// <summary>
		/// Plot width in pixels.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Plot height in pixels.
		/// </summary>
		public int Height { get; set; }
	}
}
