using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// A request to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotRequest
	{
		/// <summary>
		/// Image width.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(1024)]
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(768)]
		public int Height { get; set; }

		/// <summary>
		/// Parameters with which to plot the image.
		/// </summary>
		[Required]
		public MandelbrotParameters? Parameters { get; set; }
	}
}
