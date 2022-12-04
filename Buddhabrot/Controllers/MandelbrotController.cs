using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Buddhabrot.API.Controllers
{
	/// <summary>
	/// Mandelbrot API controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class MandelbrotController : ControllerBase
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
		/// Instantiates a <see cref="MandelbrotController"/>.
		/// </summary>
		/// <param name="repository">Plot repository.</param>
		/// <param name="mapper">AutoMapper.</param>
		public MandelbrotController(IPlotRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		/// <summary>
		/// Plots a Mandelbrot image.
		/// </summary>
		/// <param name="parameters">Image generation parameters.</param>
		/// <returns>A Mandelbrot image.</returns>
		[HttpPost("Plot")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<IActionResult> Plot(MandelbrotParameters parameters)
		{
			var plotParameters = _mapper.Map<Core.Models.MandelbrotParameters>(parameters);
			var plot = new Core.Models.Plot(plotParameters, Core.Models.PlotType.Mandelbrot);

			_repository.Add(plot);
			await _repository.SaveChangesAsync();
			await _repository.EnqueuePlot(plot.Id);

			return Created("api/image/{id}", new { id = plot.Id });
		}
	}
}
