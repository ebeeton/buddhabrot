using Buddhabrot.Core.Interfaces;
using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Contexts;
using Buddhabrot.Persistence.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Buddhabrot.Persistence.Repositories
{
	/// <summary>
	/// Buddhabrot repository.
	/// </summary>
	public class PlotRepository : IPlotRepository
	{
		/// <summary>
		/// Database context.
		/// </summary>
		protected readonly BuddhabrotContext _context;

		/// <summary>
		/// Instantiates a <see cref="PlotRepository"/>.
		/// </summary>
		/// <param name="context"><see cref="BuddhabrotContext"/>.</param>
		public PlotRepository(BuddhabrotContext context) => _context = context;

		/// <summary>
		/// Add a <see cref="Plot"/> to the repository.
		/// </summary>
		/// <param name="plot"><see cref="Plot"/></param>
		public void Add(Plot plot)
		{
			_context.Plots.Add(plot);
		}

		/// <summary>
		/// Saves all changes made in this context.
		/// </summary>
		/// <returns>A task that represents the asynchronous save operation.
		/// The task result contains the number of state entries written to the database.
		/// </returns>
		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Enqueue a <see cref="IPlotParameters"/>.
		/// </summary>
		/// <param name="plotParameters"><see cref="IPlotParameters"/>.</param>
		/// <returns>A task representing the work to enqueue the <see cref="IPlotParameters"/>.</returns>
		public async Task EnqueuePlotRequest(IPlotParameters plotParameters)
		{
			await _context.EnqueuePlotRequest(plotParameters);
		}

		/// <summary>
		/// Dequeues the next pending plot parameters.
		/// </summary>
		/// <returns>A task representing the work to dequeue the plot parameters.</returns>
		public IPlotParameters? DequeuePlotRequest()
		{

			var json = _context.DequeuePlotRequest();
			if (json == null)
			{
				// Not an error case, there's just nothing to do.
				return null;
			}

			var jObject = JObject.Parse(json);
			if (jObject is null)
			{
				throw new InvalidOperationException("Failed to parse plot parameters.");
			}

			var type = (string?)jObject["Type"] ?? throw new InvalidOperationException("Type property not found.");
			return Enum.Parse<PlotType>(type) switch
			{
				PlotType.Mandelbrot => JsonConvert.DeserializeObject<MandelbrotParameters>(json),
				PlotType.Buddhabrot => JsonConvert.DeserializeObject<BuddhabrotParameters>(json),
				_ => throw new InvalidOperationException($"Unsupported plot type \"{type}\"."),
			};
		}
	}
}
