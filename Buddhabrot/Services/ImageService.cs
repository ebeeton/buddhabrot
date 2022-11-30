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
		/// <param name="plot"><see cref="Plot"/>.</param>
		/// <returns>A task representing the work to obtain a <see cref="MemoryStream"/> containing the PNG.</returns>
		public static async Task<MemoryStream> ToPng(Plot plot)
		{
			using var image = Image.LoadPixelData<Rgb24>(plot.ImageData, plot.Width, plot.Height);
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);
			return output;
		}
	}
}
