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
		/// Enqueue a <see cref="Plot"/> ID for future processing.
		/// </summary>
		/// <param name="plotId"><see cref="Plot"/> ID</param>
		/// <returns>A task representing the work to enqueue the <see cref="Plot"/> ID.</returns>
		public Task EnqueuePlot(int plotId);

		/// <summary>
		/// Dequeues the next pending plot <see cref="Plot"/>.
		/// </summary>
		/// <returns>The next pending <see cref="Plot"/>, or null if the queue is empty.</returns>
		public Plot? DequeuePlot();

		/// <summary>
		/// Find a <see cref="Plot"/> by its ID.
		/// </summary>
		/// <param name="id"><see cref="Plot"/> ID.</param>
		/// <returns>The <see cref="Plot"/>, or null if it was not found.</returns>
		public Plot? Find(int id);
	}
}
