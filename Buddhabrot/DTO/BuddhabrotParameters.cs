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
		[Range(16384, 131072)]
		[DefaultValue(16384)]
		public int MaxIterations { get; set; }

		/// <summary>
		/// Maximum number of iterations for the intial sample set.
		/// </summary>
		[Range(128, 2048)]
		[DefaultValue(2048)]
		public int MaxSampleIterations { get; set; }

		/// <summary>
		/// Size of the random sample set as a percentage of the total
		/// number of pixels in the image to be generated.
		/// </summary>
		[Range(0.01, 0.1)]
		[DefaultValue(0.01)]
		public double SampleSize { get; set; }

		/// <summary>
		/// Number of passes to build the image.
		/// </summary>
		[Range(1, 256)]
		[DefaultValue(32)]
		public int Passes { get; set; }

		/// <summary>
		/// Whether to render the same pixel value to each channel.
		/// </summary>
		[DefaultValue(false)]
		public bool Grayscale { get; set; }
	}
}
