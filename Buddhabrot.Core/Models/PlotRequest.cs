namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A request to plot an image.
	/// </summary>
	public class PlotRequest
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// When the plot was queued.
		/// </summary>
		public DateTime QueuedUTC { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// The parameters used to generate the plot, in JSON.
		/// </summary>
		public string? PlotParams { get; set; }
	}
}
