using Buddhabrot.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
		/// Enqueue a <see cref="PlotRequest"/>.
		/// </summary>
		/// <param name="plotRequest"><see cref="PlotRequest"/>.</param>
		/// <returns>A task representing the work to enqueue the <see cref="PlotRequest"/>.</returns>
		public async Task EnqueuePlotRequest(PlotRequest plotRequest)
        {
            await Database.ExecuteSqlRawAsync("EXEC uspEnqueuePlot @Type, @PlotParams",
                new SqlParameter("@Type", plotRequest.Type.ToString()),
                new SqlParameter("@PlotParams", plotRequest.PlotParams));
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

            modelBuilder.Entity<PlotRequest>()
                .Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<PlotType>())
                .HasMaxLength(10);

			base.OnModelCreating(modelBuilder);
		}
	}
}
