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
		public async Task Get_WithId_ReturnsFile()
		{
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.Find(1))
				.Returns(new Plot
				{
					Height = 1,
					Width = 1,
					ImageData = new byte[3] { 1, 1, 1 },
				})
				.Verifiable();
			var id = 1;
			var controller = new ImageController(repository.Object);

			var result = await controller.Get(id) as FileStreamResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}
	}
}
