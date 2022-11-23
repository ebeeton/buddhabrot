using Buddhabrot.Core.Models;

namespace Buddhabrot.Persistence
{
	/// <summary>
	/// A Buddhabrot plot.
	/// </summary>
	public class BuddhabrotPlot
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The parameters used to generate the plot.
		/// </summary>
		public BuddhabrotParameters? Parameters { get; set; }

		/// <summary>
		/// When the record was created.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// When the plot began.
		/// </summary>
		public DateTime PlotBeginUTC { get; set; }

		/// <summary>
		/// When the plot ended.
		/// </summary>
		public DateTime PlotEndUTC { get; set; }

		/// <summary>
		/// The plotted image data.
		/// </summary>
		public byte[]? PngImageData { get; set; }
	}
}