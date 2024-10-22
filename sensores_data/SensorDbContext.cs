using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace sensores_data
{
    public class SensorDbContext : DbContext
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options)
        {
        }
        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Id as primary key (you can customize further if needed).
            modelBuilder.Entity<SensorData>().HasKey(s => s.Id);
            base.OnModelCreating(modelBuilder);

        }
    }
}
