using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.API.Services;
using Buddhabrot.Core.Plotting;
using Buddhabrot.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Plot(MandelbrotParameters parameters)
		{
			try
			{
				var plot = new Core.Models.Plot
				{
					Height = parameters.Height,
					Width = parameters.Width,
					PlotParams = JsonConvert.SerializeObject(parameters),
					PlotType = Core.Models.PlotType.Mandelbrot,
				};

				_repository.Add(plot);
				await _repository.SaveChangesAsync();
				await _repository.EnqueuePlot(plot.Id);

				var plotParameters = _mapper.Map<Core.Models.MandelbrotParameters>(parameters);
				var plotter = new MandelbrotPlotter(plotParameters);
				var temp = await plotter.Plot();

				return File(await ImageService.ToPng(temp), ImageService.PngContentType);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Mandelbrot plot failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
