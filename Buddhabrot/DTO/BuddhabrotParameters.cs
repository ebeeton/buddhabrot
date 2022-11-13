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
		/// Image width.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(1024)]
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(768)]
		public int Height { get; set; }

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
		[Range(0.1, 0.5)]
		[DefaultValue(0.1)]
		public double SampleSize { get; set; }
	}
}
