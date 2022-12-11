using AutoMapper;
using Buddhabrot.API.Controllers;
using Buddhabrot.API.DTO;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Buddhabrot.Test.API.Controller
{
	[TestClass]
	public class PlotsControllerTests
	{
		private readonly IMapper _mapper;

		public PlotsControllerTests()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AutoMapperProfile());
			});
			_mapper = config.CreateMapper();
		}

		[TestMethod]
		public async Task PlotAsync_WithMandelbrotRequest_ReturnsCreated()
		{
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.SaveChangesAsync())
				.Verifiable();
			var request = new MandelbrotRequest
			{
				Height = 1024,
				Width = 768,
				Parameters = new MandelbrotParameters
				{
					MaxIterations = 128
				}
			};
			var controller = new PlotsController(repository.Object, _mapper);

			var result = await controller.PlotAsync(request) as CreatedAtRouteResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task PlotAsync_WithBuddhabrotRequest_ReturnsCreated()
		{
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.SaveChangesAsync())
				.Verifiable();
			var request = new BuddhabrotRequest
			{
				Height = 1024,
				Width = 768,
				Parameters = new BuddhabrotParameters
				{
					MaxIterations = 128,
					MaxSampleIterations = 128,
					SampleSize = 1024
				}
			};
			var controller = new PlotsController(repository.Object, _mapper);

			var result = await controller.PlotAsync(request) as CreatedAtRouteResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task GetAsync_WithValidId_ReturnsFileStreamResult()
		{
			var id = 1;
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.FindAsync(id))
				.ReturnsAsync(new Buddhabrot.Core.Models.Plot
				{
					Height = 1,
					Id = 1,
					Width = 1,
					ImageData = new byte[3] { 1, 1, 1 },
				})
				.Verifiable();
			var controller = new PlotsController(repository.Object, _mapper);

			var result = await controller.GetAsync(id) as FileStreamResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}

		[TestMethod]
		public async Task GetAsync_WithNoImageData_ReturnsAccepted()
		{
			var id = 1;
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.FindAsync(id))
				.ReturnsAsync(new Buddhabrot.Core.Models.Plot
				{
					Height = 1,
					Id = 1,
					Width = 1,
					ImageData = null,
				})
				.Verifiable();
			var controller = new PlotsController(repository.Object, _mapper);

			var result = await controller.GetAsync(id) as AcceptedResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}

		[TestMethod]
		public async Task GetAsync_WithInvalidId_ReturnsNotFound()
		{
			var id = 1;
			var repository = new Mock<IPlotRepository>();
			repository.Setup(r => r.FindAsync(id))
				.ReturnsAsync(() => null)
				.Verifiable();
			var controller = new PlotsController(repository.Object, _mapper);

			var result = await controller.GetAsync(id) as NotFoundResult;

			Assert.IsNotNull(result);
			repository.Verify();
		}
	}
}
