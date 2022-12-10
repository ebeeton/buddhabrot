using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.API.Services;
using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Mandelbrot API controller.
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
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Plot(MandelbrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			return Created("api/image/{id}", new { id = plot.Id });
		}

		/// <summary>
		/// Plots a Buddhabrot image.
		/// </summary>
		/// <param name="request">Buddhabrot plot request.</param>
		/// <returns>The ID of the queued plot.</returns>
		[HttpPost("Buddhabrot")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Plot(BuddhabrotRequest request)
		{
			var plot = _mapper.Map<Plot>(request);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			return Created("api/image/{id}", new { id = plot.Id });
		}

		/// <summary>
		/// Gets an image.
		/// </summary>
		/// <returns>A task representing the work to get the image.</returns>
		[HttpGet("{id}", Name = "GetImage")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var plot = await _repository.FindAsync(id);
			if (plot == null)
			{
				return new NotFoundResult();
			}
			else if (plot.ImageData == null)
			{
				// Still proccessing.
				return new AcceptedResult();
			}

			return File(await ImageService.ToPng(plot), ImageService.PngContentType);
		}
	}
}
