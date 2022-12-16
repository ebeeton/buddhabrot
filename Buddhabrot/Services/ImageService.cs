using Buddhabrot.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Buddhabrot.API.Services
{
	/// <summary>
	/// Image conversion services.
	/// </summary>
	public static class ImageService
	{
		/// <summary>
		/// PNG image format MIME type.
		/// </summary>
		public const string PngContentType = "image/png";

		/// <summary>
		/// Generate a plot PNG image from a <see cref="Plot"/>.
		/// </summary>
		/// <param name="imageRgb"><see cref="ImageRgb"/>.</param>
		/// <returns>A task representing the work to obtain a <see cref="MemoryStream"/> containing the PNG.</returns>
		public static async Task<MemoryStream> ToPng(ImageRgb imageRgb)
		{
			using var image = Image.LoadPixelData<Rgb24>(imageRgb.Data, imageRgb.Width, imageRgb.Height);
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);
			return output;
		}
	}
}
