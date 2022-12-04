namespace Buddhabrot.Core.Interfaces
{
	/// <summary>
	/// Parameters used to generate a plot.
	/// </summary>
	public interface IPlotParameters
	{
		/// <summary>
		/// Maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }
	}
}
