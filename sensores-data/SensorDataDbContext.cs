using Microsoft.EntityFrameworkCore;

namespace sensores_data
{
    public class SensorDataDbContext : DbContext
    {
        public SensorDataDbContext(DbContextOptions<SensorDataDbContext> context) : base(context)
        {
        }
        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define a entidade como sem chave primária (keyless)
            modelBuilder.Entity<SensorData>()
                        .HasNoKey();
        }
    }
}
