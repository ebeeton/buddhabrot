using Buddhabrot.Core.Models;

namespace Buddhabrot.Persistence.Interfaces
{
	/// <summary>
	/// Buddhabrot repository.
	/// </summary>
	public interface IPlotRepository
	{
		/// <summary>
		/// Add a <see cref="Plot"/> to the repository.
		/// </summary>
		/// <param name="plot"><see cref="Plot"/></param>
		public void Add(Plot plot);

		/// <summary>
		/// Saves all changes made in this context.
		/// </summary>
		/// <returns>A task that represents the asynchronous save operation.
		/// The task result contains the number of state entries written to the database.
		/// </returns>
		public Task<int> SaveChangesAsync();

		/// <summary>
		/// Enqueue a <see cref="PlotRequest"/>.
		/// </summary>
		/// <param name="plotRequest"><see cref="PlotRequest"/>.</param>
		/// <returns>A task representing the work to enqueue the <see cref="PlotRequest"/>.</returns>
		public Task EnqueuePlotRequest(PlotRequest plotRequest);
	}
}
