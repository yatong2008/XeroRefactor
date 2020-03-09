using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XeroRefactor.Models;

namespace XeroRefactor.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAllByNameAsync(string name);
        Task<Product> GetByIdAsync(Guid id);
        Task<Product> CreateAsync(Product model);
        Task UpdateAsync(Product model);
        Task DeleteAsync(Guid id);
    }
}
