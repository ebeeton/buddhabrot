namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A request to plot an image.
	/// </summary>
	public class PlotRequest
	{
		/// <summary>
		/// <see cref="Plot"/> ID.
		/// </summary>
		public int PlotId { get; set; }

		/// <summary>
		/// When the plot was queued.
		/// </summary>
		public DateTime QueuedUTC { get; set; } = DateTime.UtcNow;
	}
}
