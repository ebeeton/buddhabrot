namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Response to a request to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotResponse
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Image width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Parameters with which to plot the image.
		/// </summary>
		public BuddhabrotParameters? Parameters { get; set; }

		/// <summary>
		/// State of the plot request.
		/// </summary>
		public string? State { get; set; }
	}
}
