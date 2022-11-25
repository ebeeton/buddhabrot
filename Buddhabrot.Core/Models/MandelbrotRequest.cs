namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A request to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotRequest
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
		public MandelbrotParameters? Parameters { get; set; }
	}
}
