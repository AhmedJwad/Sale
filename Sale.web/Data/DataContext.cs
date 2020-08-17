

using Microsoft.EntityFrameworkCore;
using Sale.Common.Entities;

namespace Sale.web.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext>options):base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City>Cities { get; set; }
        public DbSet<Department>Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>()
               .HasIndex(c => c.Name)
               .IsUnique();
            modelBuilder.Entity<City>()
              .HasIndex(c => c.Name)
              .IsUnique();
            modelBuilder.Entity<Department>()
             .HasIndex(c => c.Name)
             .IsUnique();
        }

    }
}
