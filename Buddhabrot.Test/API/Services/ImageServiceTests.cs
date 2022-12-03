using Buddhabrot.API.Services;
using Buddhabrot.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Buddhabrot.Test.API.Services
{
	[TestClass]
	public class ImageServiceTests
	{
		/// <summary>
		/// 1x1 pixel RGB PNG image data.
		/// </summary>
		private readonly byte[] _png1x1 = new byte[90] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 1, 0, 0, 0, 1, 8, 2, 0, 0, 0, 144, 119, 83, 222, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 196, 0, 0, 14, 196, 1, 149, 43, 14, 27, 0, 0, 0, 12, 73, 68, 65, 84, 120, 156, 99, 97, 100, 100, 4, 0, 0, 26, 0, 8, 101, 156, 114, 55, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };
	
		[TestMethod]
		public async Task ToPng_WithPlot_ReturnsPng()
		{
			var plot = new Plot
			{
				Height = 1,
				Width = 1,
				ImageData = new byte[] { 1, 1, 1 }
			};

			var stream = await ImageService.ToPng(plot);

			Assert.That.AreEqual(_png1x1, stream.ToArray());
		}
	}
}
