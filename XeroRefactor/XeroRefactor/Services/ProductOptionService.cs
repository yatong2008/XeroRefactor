using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XeroRefactor.Models;
using XeroRefactor.Repositories;

namespace XeroRefactor.Services
{
    public class ProductOptionService : IProductOptionService
    {
        private readonly IProductOptionRepository _repository;

        public ProductOptionService(IProductOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductOption>> GetByProductIdAsync(Guid productId)
        {
            return await _repository.GetProductOptions(productId);
        }

        public async Task<ProductOption> GetByProductOptionIdAsync(Guid productId, Guid id)
        {
            return await _repository.GetProductOption(productId, id);
        }

        public async Task<ProductOption> AddAsync(Guid productId, ProductOption model)
        {
            var productOption = await _repository.CreateProductOption(model);
            return productOption;
        }

        public async Task UpdateAsync(Guid id, ProductOption model)
        {
            await _repository.UpdateProductOption(model);
        }

        public async Task DeleteAsync(Guid productId, Guid id)
        {
            await _repository.DeleteProductOption(productId, id);
        }
    }
}
