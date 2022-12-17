using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.API.Services;
using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Plots controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class PlotsController : ControllerBase
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
		/// Instantiates a <see cref="PlotsController"/>.
		/// </summary>
		/// <param name="repository">Plot repository.</param>
		/// <param name="mapper">AutoMapper.</param>
		public PlotsController(IPlotRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Plots a Mandelbrot image.
		/// </summary>
		/// <param name="request">Mandelbrot plot request.</param>
		/// <returns>The ID of the queued plot.</returns>
		[HttpPost("Mandelbrot")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> PlotAsync(MandelbrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlotAsync(plot.Id);

			var response = _mapper.Map<MandelbrotResponse>(plot);
			return AcceptedAtRoute("GetPlot", new { id = response.Id }, response);
		}

		/// <summary>
		/// Plots a Buddhabrot image.
		/// </summary>
		/// <param name="request">Buddhabrot plot request.</param>
		/// <returns>The ID of the queued plot.</returns>
		[HttpPost("Buddhabrot")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> PlotAsync(BuddhabrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlotAsync(plot.Id);

			var response = _mapper.Map<BuddhabrotResponse>(plot);
			return AcceptedAtRoute("GetPlot", new { id = plot.Id }, response);
		}

		/// <summary>
		/// Gets a plot.
		/// </summary>
		/// <returns>A task representing the work to get the plot.</returns>
		[HttpGet("{id}", Name = "GetPlot")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAsync(int id)
		{
			var plot = await _repository.FindAsync(id);
			if (plot == null)
			{
				return new NotFoundResult();
			}

			if (plot.PlotType == PlotType.Buddhabrot)
			{
				return Ok(_mapper.Map<BuddhabrotResponse>(plot));
			}
			else
			{
				return Ok(_mapper.Map<MandelbrotResponse>(plot));
			}
		}
	}
}
