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
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Plot(BuddhabrotParameters parameters)
		{
			try
			{
				var plotParameters = _mapper.Map<Core.Models.BuddhabrotParameters>(parameters);
				var plot = new Core.Models.Plot
				{
					Height = plotParameters.Height,
					Width = plotParameters.Width,
					PlotParams = JsonConvert.SerializeObject(plotParameters),
					PlotType = Core.Models.PlotType.Buddhabrot,
				};

				_repository.Add(plot);
				await _repository.SaveChangesAsync();
				await _repository.EnqueuePlot(plot.Id);
				var plotter = new BuddhabrotPlotter(plotParameters);
				var temp = await plotter.Plot();

				return File(await ImageService.ToPng(temp), ImageService.PngContentType);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Buddhabrot plot failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
