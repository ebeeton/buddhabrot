using Buddhabrot.Core.Interfaces;
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
		/// Enqueue a <see cref="IPlotParameters"/>.
		/// </summary>
		/// <param name="plotParameters"><see cref="IPlotParameters"/>.</param>
		/// <returns>A task representing the work to enqueue the <see cref="IPlotParameters"/>.</returns>
		public Task EnqueuePlotRequest(IPlotParameters plotParameters);

		/// <summary>
		/// Dequeues the next pending <see cref="IPlotParameters"/>.
		/// </summary>
		/// <returns>A task representing the work to dequeue the <see cref="IPlotParameters"/>.</returns>
		public IPlotParameters? DequeuePlotRequest();
	}
}
