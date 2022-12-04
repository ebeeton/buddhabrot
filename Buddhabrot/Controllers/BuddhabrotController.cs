using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Buddhabrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class BuddhabrotController : ControllerBase
	{
		/// <summary>
		/// AutoMapper.
		/// </summary>
		protected IMapper _mapper;

		/// <summary>
		/// Plot repository.
		/// </summary>
		protected IPlotRepository _repository;

		/// <summary>
		/// Instantiates a <see cref="BuddhabrotController"/>.
		/// </summary>
		/// <param name="mapper">AutoMapper.</param>
		/// <param name="repository">Plot repository.</param>
		public BuddhabrotController(IPlotRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Plots a Buddhabrot image.
		/// </summary>
		/// <param name="parameters">Image generation parameters.</param>
		/// <returns>A Buddhabrot image.</returns>
		[HttpPost("Plot")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Plot(BuddhabrotParameters parameters)
		{
			var plotParameters = _mapper.Map<Core.Models.BuddhabrotParameters>(parameters);
			var plot = new Core.Models.Plot(plotParameters, Core.Models.PlotType.Buddhabrot);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			return Created("api/image/{id}", new { id = plot.Id });
		}
	}
}
