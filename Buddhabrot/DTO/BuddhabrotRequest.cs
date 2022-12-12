using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// A request to plot a Buddhabrot image.
	/// </summary>
	public class BuddhabrotRequest
	{
		/// <summary>
		/// Image width.
		/// </summary>
		[Range(128, 16384)]
		[DefaultValue(4096)]
		public int Width { get; set; }

		/// <summary>
		/// Image height.
		/// </summary>
		[Range(128, 16384)]
		[DefaultValue(4096)]
		public int Height { get; set; }

		/// <summary>
		/// Parameters with which to plot the image.
		/// </summary>
		[Required]
		public BuddhabrotParameters? Parameters { get; set; }
	}
}
