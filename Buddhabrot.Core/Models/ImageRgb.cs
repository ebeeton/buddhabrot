namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// A raw 24-bit per pixel image.
	/// </summary>
	public class ImageRgb
	{
		/// <summary>
		/// The number of bytes in a 24 bit RGB pixel.
		/// </summary>
		public const int BytesPerPixel = 3;

		/// <summary>
		/// Instantiates an <see cref="ImageRgb"/>.
		/// </summary>
		public ImageRgb()
		{
			Data = new byte[0];
		}

		/// <summary>
		/// Instantiates an <see cref="ImageRgb"/>.
		/// </summary>
		/// <param name="width">Image width in pixels.</param>
		/// <param name="height">Image height in pixels.</param>
		public ImageRgb(int width, int height)
		{
			Width = width;
			Height = height;
			BytesPerLine = width * BytesPerPixel;
			Data = new byte[height * BytesPerLine];
		}

		/// <summary>
		/// ID of the <see cref="Plot"/> that generated the image.
		/// </summary>
		public int PlotId { get; set; }

		/// <summary>
		/// Number of bytes per line of the image.
		/// </summary>
		public int BytesPerLine { get; set; }

		/// <summary>
		/// Image width in pixels.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Image height in pixels.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Image data.
		/// </summary>
		public byte[] Data { get; set; }
	}
}
