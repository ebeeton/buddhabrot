using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Parameters with which to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotParameters
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
		/// Maximum number of iterations for each pixel.
		/// </summary>
		[Range(128, 2048)]
		[DefaultValue(128)]
		public int MaxIterations { get; set; }
	}
}
