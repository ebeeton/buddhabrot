using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Parameters with which to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotParameters
	{
		/// <summary>
		/// Maximum number of iterations for each pixel.
		/// </summary>
		[Range(1, int.MaxValue)]
		[DefaultValue(10000)]
		public int MaxIterations { get; set; }

		/// <summary>
		/// Maximum number of iterations for the intial sample set.
		/// </summary>
		[Range(1, int.MaxValue)]
		[DefaultValue(1000)]
		public int MaxSampleIterations { get; set; }

		/// <summary>
		/// Number of random points on the complex plane to plot with.
		/// </summary>
		[Range(1, int.MaxValue)]
		[DefaultValue(25000000)]
		public int SampleSize { get; set; }
	}
}
