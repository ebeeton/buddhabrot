namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// Parameters with which to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotParameters
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
		/// Maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }
	}
}
