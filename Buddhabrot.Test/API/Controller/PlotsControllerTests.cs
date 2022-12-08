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
		public async Task MandelbrotPlot_WithValidRequest_ReturnsCreated()
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

			var result = await controller.Plot(request) as CreatedResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task BuddhabrotPlot_WithValidRequest_ReturnsCreated()
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

			var result = await controller.Plot(request) as CreatedResult;

			Assert.IsNotNull(result);
		}
	}
}
