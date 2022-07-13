using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Parameters with which to plot a fractal image.
	/// </summary>
	public class PlotParameters
	{
		/// <summary>
		/// Gets/sets the image width.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(1024)]
		public int Width { get; set; }

		/// <summary>
		/// Gets/sets the image height.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(768)]
		public int Height { get; set; }

		/// <summary>
		/// Gets/sets the maximum number of iterations for each pixel.
		/// </summary>
		[Range(128, 2096)]
		[DefaultValue(128)]
		public int MaxIterations { get; set; }
	}
}
