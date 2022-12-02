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
		/// Enqueue a <see cref="Plot"/> ID for future processing.
		/// </summary>
		/// <param name="plotId"><see cref="Plot"/> ID</param>
		/// <returns>A task representing the work to enqueue the <see cref="Plot"/> ID.</returns>
		public async Task EnqueuePlot(int plotId)
		{
            await Database.ExecuteSqlRawAsync("EXEC uspEnqueuePlot @PlotID",
                new SqlParameter("@PlotID", plotId));
		}

		/// <summary>
		/// Dequeues the next <see cref="Plot"/> ID.
		/// </summary>
		/// <returns>Plot ID, or null if the queue is empty.</returns>
		public int? DequeuePlotId()
        {
            return Database.SqlQuery<int?>($"EXEC uspDequeuePlot").AsEnumerable().FirstOrDefault();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<PlotRequest>()
                .ToTable("PlotQueue")
                .HasKey(q => q.PlotId)
                .IsClustered();

            modelBuilder.Entity<PlotRequest>()
                .HasOne<Plot>()
                .WithOne()
                .HasForeignKey<PlotRequest>(p => p.PlotId);

            modelBuilder.Entity<Plot>()
                .Property(p => p.PlotType)
                .HasConversion(new EnumToStringConverter<PlotType>());

			base.OnModelCreating(modelBuilder);
		}
	}
}
