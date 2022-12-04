using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
		/// <param name="request">Buddhabrot plot request.</param>
		/// <returns>The ID of the queued plot.</returns>
		[HttpPost("Plot")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Plot(BuddhabrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			return Created("api/image/{id}", new { id = plot.Id });
		}
	}
}
