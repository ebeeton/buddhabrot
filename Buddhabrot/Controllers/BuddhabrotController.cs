using AutoMapper;
using Buddhabrot.API.DTO;
using Buddhabrot.Core.Plotting;
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
		/// Instantiates a <see cref="BuddhabrotController"/>.
		/// </summary>
		/// <param name="mapper">AutoMapper.</param>
		public BuddhabrotController(IMapper mapper) => _mapper = mapper;

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
				var stopwatch = System.Diagnostics.Stopwatch.StartNew();
				var plotter = new BuddhabrotPlotter(_mapper.Map<Core.Parameters.BuddhabrotParameters>(parameters));
				var image = await plotter.PlotPng();
				Log.Information($"Plotted image in {stopwatch.ElapsedMilliseconds} ms.");
				return File(image, Constants.PngContentType);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Buddhabrot plot failed.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
