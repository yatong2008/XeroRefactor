using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using XeroRefactor.Models;

namespace XeroRefactor.Data
{
    public interface IProductContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductOption> ProductOptions { get; set; }
        Task SaveChangesAsync();
        EntityEntry Entry(object entity);
        ValueTask<EntityEntry> AddAsync(object entity);
    }
}