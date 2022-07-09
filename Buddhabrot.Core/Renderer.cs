namespace Buddhabrot.Core
{
	/// <summary>
	/// A fractal image renderer.
	/// </summary>
	public abstract class Renderer
	{
		/// <summary>
		/// Instantiates fractal a renderer.
		/// </summary>
		/// <param name="maxIterations">Maximum number of iterations for each pixel.</param>
		public Renderer(int maxIterations)
		{
			MaxIterations = maxIterations;
		}

		protected const int RGBBytesPerPixel = 3;
		protected readonly int MaxIterations;
	}
}