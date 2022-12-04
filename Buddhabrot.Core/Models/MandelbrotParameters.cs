using Buddhabrot.Core.Interfaces;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// Parameters with which to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotParameters : IPlotParameters
	{
		/// <summary>
		/// Maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }
	}
}
