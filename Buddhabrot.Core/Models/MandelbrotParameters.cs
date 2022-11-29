using Buddhabrot.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// Parameters with which to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotParameters : IPlotParameters
	{
		/// <summary>
		/// Image width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Type of plot.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public PlotType Type { get => PlotType.Mandelbrot; set { } }

		/// <summary>
		/// Maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }
	}
}
