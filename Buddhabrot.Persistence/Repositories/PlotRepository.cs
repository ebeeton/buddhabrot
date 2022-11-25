using Buddhabrot.Core.Models;
using Buddhabrot.Persistence.Contexts;
using Buddhabrot.Persistence.Interfaces;

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
		/// Add a <see cref="BuddhabrotPlot"/> to the repository.
		/// </summary>
		/// <param name="plot"><see cref="BuddhabrotPlot"/></param>
		public void Add(BuddhabrotPlot plot)
		{
			_context.BuddhabrotPlots.Add(plot);
		}

		/// <summary>
		/// Add a <see cref="MandelbrotPlot"/> to the repository.
		/// </summary>
		/// <param name="plot"><see cref="MandelbrotPlot"/></param>
		public void Add(MandelbrotPlot plot)
		{
			_context.MandelbrotPlots.Add(plot);
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
	}
}
