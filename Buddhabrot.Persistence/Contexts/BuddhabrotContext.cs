using Buddhabrot.Core.Models;
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<BuddhabrotRequest>().OwnsOne(
                plot => plot.Parameters, builder => builder.ToJson())
                .ToTable("BuddhabrotQueue")
                .HasKey(q => q.Id)
                .IsClustered();

			modelBuilder.Entity<MandelbrotRequest>().OwnsOne(
                plot => plot.Parameters, builder => builder.ToJson())
				.ToTable("MandelbrotQueue")
				.HasKey(q => q.Id)
				.IsClustered();

            modelBuilder.Entity<Plot>()
                .Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<Plot.PlotType>());

			base.OnModelCreating(modelBuilder);
		}
	}
}
