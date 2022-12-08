using Buddhabrot.API.Controllers;
using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Buddhabrot.Test.API.Controller
{
	[TestClass]
	public class ImageControllerTests
	{
		[TestMethod]
		public async Task Get_WithValidId_ReturnsFileStreamResult()
		{
			var id = 1;
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.FindAsync(id))
				.ReturnsAsync(new Plot
				{
					Height = 1,
					Id = 1,
					Width = 1,
					ImageData = new byte[3] { 1, 1, 1 },
				})
				.Verifiable();
			var controller = new ImageController(repository.Object);

			var result = await controller.Get(id) as FileStreamResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}

		[TestMethod]
		public async Task Get_WithInvalidId_ReturnsNotFound()
		{
			var id = 1;
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.FindAsync(id))
				.ReturnsAsync(() => null)
				.Verifiable();
			var controller = new ImageController(repository.Object);

			var result = await controller.Get(id) as NotFoundResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}
	}
}
