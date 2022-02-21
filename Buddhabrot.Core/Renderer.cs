using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Buddhabrot.Core
{
	public class Renderer
	{
		/// <summary>
		/// Test render method.
		/// </summary>
		/// <returns>A task representing the work to render the image.</returns>
		public static async Task<MemoryStream> RenderPng()
		{
			const int width = 512, height = 384, rowSize = width * 3;

			var data = new byte[height * rowSize];

			for (int y = 0; y < height; ++y)
				for (int x = 0; x < rowSize; x += 3)
				{
					var index = y * rowSize + x;
					data[index] = (byte)((float)x / rowSize * 255);
				}

			using var image = Image.LoadPixelData<Rgb24>(data, width, height);
			var output = new MemoryStream();
			await image.SaveAsPngAsync(output);
			output.Seek(0, SeekOrigin.Begin);

			return output;
		}
	}
}