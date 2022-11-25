using Buddhabrot.Core.Models;

namespace Buddhabrot.Persistence.Interfaces
{
	/// <summary>
	/// Buddhabrot repository.
	/// </summary>
	public interface IPlotRepository
	{
		/// <summary>
		/// Add a <see cref="BuddhabrotPlot"/> to the repository.
		/// </summary>
		/// <param name="plot"><see cref="BuddhabrotPlot"/></param>
		public void Add(BuddhabrotPlot plot);

		/// <summary>
		/// Add a <see cref="MandelbrotPlot"/> to the repository.
		/// </summary>
		/// <param name="plot"><see cref="MandelbrotPlot"/></param>
		public void Add(MandelbrotPlot plot);

		/// <summary>
		/// Saves all changes made in this context.
		/// </summary>
		/// <returns>A task that represents the asynchronous save operation.
		/// The task result contains the number of state entries written to the database.
		/// </returns>
		public Task<int> SaveChangesAsync();
	}
}
