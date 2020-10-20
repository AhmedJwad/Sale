

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sale.Common.Entities;
using Sale.web.Data.Entities;

namespace Sale.web.Data
{
    public class DataContext:IdentityDbContext<User>

    {
        public DataContext(DbContextOptions<DataContext>options):base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City>Cities { get; set; }
        public DbSet<Department>Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
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
            modelBuilder.Entity<Category>()
           .HasIndex(c => c.Name)
           .IsUnique();
            modelBuilder.Entity<Product>()
         .HasIndex(c => c.Name)
         .IsUnique();
        }

    }
}
