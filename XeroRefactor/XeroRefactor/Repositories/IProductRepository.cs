using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XeroRefactor.Models;

namespace XeroRefactor.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetByIdAsync(Guid id);
        Task<List<Product>> SearchProductsByName(string name);
        Task DeleteProduct(Guid id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProduct(Product product);
    }
}