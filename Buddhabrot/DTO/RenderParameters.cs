using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Buddhabrot.API.DTO
{
	/// <summary>
	/// Parameters with which to render a Buddhabrot image.
	/// </summary>
	public class RenderParameters
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
	}
}
