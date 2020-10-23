

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
           
            modelBuilder.Entity<Category>()
           .HasIndex(c => c.Name)
           .IsUnique();
            modelBuilder.Entity<Product>()
            .HasIndex(c => c.Name)
            .IsUnique();

            modelBuilder.Entity<Country>(cou =>
            {
                cou.HasIndex("Name").IsUnique();
                cou.HasMany(c => c.Departments).WithOne(d => d.Country)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Department>(dep =>
            {
                dep.HasIndex("Name", "CountryId").IsUnique();
                dep.HasOne(d => d.Country).WithMany(c => c.Departments)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<City>(cty =>
            {
                cty.HasIndex("Name", "DepartmentId").IsUnique();
                cty.HasOne(c => c.Department).WithMany(c => c.Cities)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
