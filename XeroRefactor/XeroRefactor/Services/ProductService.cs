using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XeroRefactor.Models;
using XeroRefactor.Repositories;

namespace XeroRefactor.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _repository.GetProducts();
        }

        public async Task<List<Product>> GetAllByNameAsync(string name)
        {
            return await _repository.SearchProductsByName(name);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(Product model)
        {
            return await _repository.CreateProductAsync(model);
        }

        public async Task UpdateAsync(Product model)
        {
            await _repository.UpdateProduct(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteProduct(id);
        }
    }
}
