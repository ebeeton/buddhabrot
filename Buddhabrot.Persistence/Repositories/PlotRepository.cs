using Buddhabrot.Core.Models;
using Buddhabrot.Models;
using Buddhabrot.Persistence.Contexts;
using Buddhabrot.Persistence.Interfaces;

namespace Buddhabrot.Persistence.Repositories
{
	public class PlotRepository : IPlotRepository
	{
		protected readonly BuddhabrotContext _context;

		public PlotRepository(BuddhabrotContext context) => _context = context;

		public void Add(BuddhabrotPlot plot)
		{
			_context.BuddhabrotPlots.Add(plot);
		}

		public void Add(MandelbrotPlot plot)
		{
			_context.MandelbrotPlots.Add(plot);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
