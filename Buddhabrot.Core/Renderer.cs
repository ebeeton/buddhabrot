using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Buddhabrot.Core
{
	/// <summary>
	/// Renders Buddhabrot images.
	/// </summary>
	public class Renderer
	{
		private const int RGBBytesPerPixel = 3;

		/// <summary>
		/// Test render method.
		/// </summary>
		/// <returns>A task representing the work to render the image.</returns>
		public static async Task<MemoryStream> RenderPng(int width, int height)
		{
			int rowLength = width * RGBBytesPerPixel;

			var data = new byte[height * rowLength];

			for (int y = 0; y < height; ++y)
				for (int x = 0; x < rowLength; x += 3)
				{
					var index = y * rowLength + x;
					data[index] = (byte)((float)x / rowLength * 255);
				}

			using var image = Image.LoadPixelData<Rgb24>(data, width, height);
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);

			return output;
		}
	}
}