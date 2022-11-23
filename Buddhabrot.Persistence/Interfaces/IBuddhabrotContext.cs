using Buddhabrot.Models;
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

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		/// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
		/// <returns>A task that represents the asynchronous save operation.
		/// The task result contains the number of state entries written to the database.
		/// </returns>
		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
