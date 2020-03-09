using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using XeroRefactor.Models;

namespace XeroRefactor.Data
{
    public class ProductContext : DbContext, IProductContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public ValueTask<EntityEntry> AddAsync(object entity)
        {
            return base.AddAsync(entity);
        }
    }
}
