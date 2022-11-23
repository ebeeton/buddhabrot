using Buddhabrot.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Buddhabrot.Persistence.Contexts
{
    /// <summary>
    /// Buddhabrot database context.
    /// </summary>
    public class BuddhabrotContext : DbContext, IBuddhabrotContext
    {
        /// <summary>
        /// Instantiates a <see cref="BuddhabrotContext"/>.
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions{TContext}"/>.</param>
        public BuddhabrotContext(DbContextOptions<BuddhabrotContext> options) : base(options)
        {

        }

        /// <summary>
        /// Buddhabrot plots.
        /// </summary>
        public DbSet<BuddhabrotPlot> BuddhabrotPlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuddhabrotPlot>().OwnsOne(
                plot => plot.Parameters, builder => builder.ToJson());

            base.OnModelCreating(modelBuilder);
        }
    }
}
