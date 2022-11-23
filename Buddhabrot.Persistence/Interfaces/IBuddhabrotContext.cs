using Microsoft.EntityFrameworkCore;

namespace Buddhabrot.Persistence.Interfaces
{
	/// <summary>
	/// Buddhabrot persistence context.
	/// </summary>
	public interface IBuddhabrotContext
	{
		/// <summary>
		/// Buddhabrot plots.
		/// </summary>
		public DbSet<BuddhabrotPlot> BuddhabrotPlots { get; set; }
	}
}
