using Buddhabrot.API.Controllers;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Buddhabrot.Test.API.Controller
{
	[TestClass]
	public class ImagesControllerTests
	{
		[TestMethod]
		public async Task GetAsync_WithValidId_ReturnsFileStreamResult()
		{
			var id = 1;
			var repository = new Mock<IImageRepository>();
			repository.Setup(r => r.Find(id))
				.Returns(new Buddhabrot.Core.Models.ImageRgb(1, 1))
				.Verifiable();
			var controller = new ImagesController(repository.Object);

			var result = await controller.GetAsync(id) as FileStreamResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}

		[TestMethod]
		public async Task GetAsync_WithInvalidId_ReturnsNotFound()
		{
			var id = 1;
			var repository = new Mock<IImageRepository>();
			repository.Setup(r => r.Find(id))
				.Returns(() => null)
				.Verifiable();
			var controller = new ImagesController(repository.Object);

			var result = await controller.GetAsync(id) as NotFoundResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}
	}
}
