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
		/// Gets/sets the image width.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(1024)]
		public int Width { get; set; }

		/// <summary>
		/// Gets/sets the image height.
		/// </summary>
		[Range(128, 4096)]
		[DefaultValue(768)]
		public int Height { get; set; }

		/// <summary>
		/// Gets/sets the maximum number of iterations for each pixel.
		/// </summary>
		[Range(2048, 131072)]
		[DefaultValue(2048)]
		public int MaxIterations { get; set; }

		/// <summary>
		/// Gets/sets the maximum number of iterations for the intial sample set.
		/// </summary>
		[Range(128, 2048)]
		[DefaultValue(2048)]
		public int MaxSampleIterations { get; set; }

		/// <summary>
		/// Gets/sets the size of the random sample set.
		/// </summary>
		[Range(8192, 524288)]
		[DefaultValue(8192)]
		public int SampleSize { get; set; }
	}
}
