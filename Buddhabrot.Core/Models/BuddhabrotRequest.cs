namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A request to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotRequest
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// When the 
		/// </summary>
		public DateTime QueuedUTC { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Parameters to generate the plot.
		/// </summary>
		public BuddhabrotParameters? Parameters { get; set; }
	}
}
