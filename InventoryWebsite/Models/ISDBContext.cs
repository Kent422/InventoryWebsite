using Microsoft.EntityFrameworkCore;

namespace InventoryWebsite.Models
{
    public class ISDBContext : DbContext
    {
        public ISDBContext(DbContextOptions<ISDBContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product>Product { get; set; }

        public DbSet<Location> Location { get; set; }
        public DbSet<Stock> Stock { get; set; }
    }
}
