namespace Buddhabrot.Core.Parameters
{
	/// <summary>
	/// Parameters with which to plot a Mandelbrot image.
	/// </summary>
	public class MandelbrotParameters
	{
		/// <summary>
		/// Gets/sets the image width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Gets/sets the image height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Gets/sets the maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }
	}
}
