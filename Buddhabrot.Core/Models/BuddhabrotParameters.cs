using Buddhabrot.Core.Interfaces;

namespace Buddhabrot.Core.Models
{
	/// <summary>
	/// Parameters with which to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotParameters : IPlotParameters
	{
		/// <summary>
		/// Maximum number of iterations for each pixel.
		/// </summary>
		public int MaxIterations { get; set; }

		/// <summary>
		/// Maximum number of iterations for the intial sample set.
		/// </summary>
		public int MaxSampleIterations { get; set; }

		/// <summary>
		/// Number of random points on the complex plane to plot with.
		/// </summary>
		public int SampleSize { get; set; }
	}
}
