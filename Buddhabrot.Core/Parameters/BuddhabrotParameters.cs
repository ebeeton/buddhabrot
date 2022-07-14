namespace Buddhabrot.Core.Parameters
{
	/// <summary>
	/// Parameters with which to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotParameters
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

		/// <summary>
		/// Gets/sets the maximum number of iterations for the intial sample set.
		/// </summary>
		public int MaxSampleIterations { get; set; }

		/// <summary>
		/// Gets/sets the size of the random sample set.
		/// </summary>
		public int SampleSize { get; set; }
	}
}
