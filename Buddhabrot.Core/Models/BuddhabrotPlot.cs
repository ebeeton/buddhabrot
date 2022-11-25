﻿namespace Buddhabrot.Core.Models
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
		/// Image width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// When the record was created.
		/// </summary>
		public DateTime CreatedUTC { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// When the plot began.
		/// </summary>
		public DateTime? PlotBeginUTC { get; set; }

		/// <summary>
		/// When the plot ended.
		/// </summary>
		public DateTime? PlotEndUTC { get; set; }

		/// <summary>
		/// The raw 24-bit per pixel image data.
		/// </summary>
		public byte[]? ImageData { get; set; }
	}
}