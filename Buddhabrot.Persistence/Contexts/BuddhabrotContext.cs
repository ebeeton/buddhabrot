using Buddhabrot.Core.Interfaces;
using Buddhabrot.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Buddhabrot.Persistence.Contexts
{
    /// <summary>
    /// Buddhabrot database context.
    /// </summary>
    public class BuddhabrotContext : DbContext
    {
        /// <summary>
        /// Instantiates a <see cref="BuddhabrotContext"/>.
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions{TContext}"/>.</param>
        public BuddhabrotContext(DbContextOptions<BuddhabrotContext> options) : base(options)
        {

        }

        /// <summary>
        /// Buddhabrot and Mandelbrot plots.
        /// </summary>
        public DbSet<Plot> Plots { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

		/// <summary>
		/// Enqueue a <see cref="IPlotParameters plotParameters"/>.
		/// </summary>
		/// <param name="plotRequest"><see cref="IPlotParameters"/>.</param>
		/// <returns>A task representing the work to enqueue the <see cref="IPlotParameters"/>.</returns>
		public async Task EnqueuePlotRequest(IPlotParameters plotParameters)
        {
            await Database.ExecuteSqlRawAsync("EXEC uspEnqueuePlot @PlotParams",
                new SqlParameter("@PlotParams", JsonConvert.SerializeObject(plotParameters)));
		}

		/// <summary>
		/// Dequeues the next pending plot parameters.
		/// </summary>
		/// <returns>Plot parameters in JSON, or null if there's nothing in the queue.</returns>
		public string? DequeuePlotRequest()
        {
            return Database.SqlQuery<string>($"EXEC uspDequeuePlot").AsEnumerable().FirstOrDefault();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PlotRequest>()
				.ToTable("PlotQueue")
				.HasKey(q => q.Id)
				.IsClustered();

			modelBuilder.Entity<Plot>()
                .Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<PlotType>())
                .HasMaxLength(10);

			base.OnModelCreating(modelBuilder);
		}
	}
}
