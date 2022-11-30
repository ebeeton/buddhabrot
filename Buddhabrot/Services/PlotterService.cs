using Buddhabrot.Persistence.Interfaces;
using Serilog;

namespace Buddhabrot.API.Services
{
	/// <summary>
	/// A service to dequeue and plot requests.
	/// </summary>
	public class PlotterService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private const int IdleMS = 1000;

		/// <summary>
		/// Instantiates a <see cref="PlotterService"/>.
		/// </summary>
		/// <param name="serviceScopeFactory"><see cref="IServiceScopeFactory"/>.</param>
		public PlotterService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

		/// <summary>
		/// Dequeue plot requests and plot them.
		/// </summary>
		/// <param name="stoppingToken"><see cref="CancellationToken"/>.</param>
		/// <returns>A task representing the work to dequeue and plot requests.</returns>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _serviceScopeFactory.CreateScope();
				var repository = scope.ServiceProvider.GetService<IPlotRepository>() ?? throw new NullReferenceException($"Failed to obtain {nameof(IPlotRepository)}");
				var task = Task.Run(() => repository.DequeuePlotRequest());
				var request = await task.WaitAsync(stoppingToken);
				if (request == null)
				{
					Thread.Sleep(IdleMS);
					continue;
				}

				Log.Information("Dequeued plot request: {@request}", request);
			}
		}
	}
}
