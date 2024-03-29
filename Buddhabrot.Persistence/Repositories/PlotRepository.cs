﻿using Buddhabrot.Core.Models;
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
		private readonly BuddhabrotContext _context;

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
		/// Enqueue a <see cref="Plot"/> ID for future processing.
		/// </summary>
		/// <param name="plotId"><see cref="Plot"/> ID</param>
		/// <returns>A task representing the work to enqueue the <see cref="Plot"/> ID.</returns>
		public async Task EnqueuePlotAsync(int plotId)
		{
			await _context.EnqueuePlot(plotId);
		}

		/// <summary>
		/// Dequeues the next pending plot <see cref="Plot"/>.
		/// </summary>
		/// <returns>A task representing the work to dequeue the next pending <see cref="Plot"/>.</returns>
		public async Task<Plot?> DequeuePlotAsync()
		{
			var id = _context.DequeuePlotId();
			if (id == null)
			{
				// The queue is empty. This is not an error condition.
				return null;
			}

			var plot = await FindAsync(id.Value) ?? throw new InvalidOperationException($"Plot ID {id} not found.");
			return plot;
		}

		/// <summary>
		/// Find a <see cref="Plot"/> by its ID.
		/// </summary>
		/// <param name="id"><see cref="Plot"/> ID.</param>
		/// <returns>A task representing the work to find the <see cref="Plot"/>.</returns>
		public async Task<Plot?> FindAsync(int id)
		{
			return await _context.Plots.FindAsync(id);
		}
	}
}
