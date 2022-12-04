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
		/// Size of the random sample set where 1.0 equals the number of pixels in the image.
		/// </summary>
		[Range(1.0, 100.0)]
		[DefaultValue(10.0)]
		public double SampleSize { get; set; }

		/// <summary>
		/// Whether to render the same pixel value to each channel.
		/// </summary>
		[DefaultValue(false)]
		public bool Grayscale { get; set; }
	}
}
