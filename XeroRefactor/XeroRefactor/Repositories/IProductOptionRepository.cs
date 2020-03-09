using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XeroRefactor.Models;

namespace XeroRefactor.Repositories
{
    public interface IProductOptionRepository
    {
        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);
        Task<ProductOption> GetProductOption(Guid productId, Guid id);
        Task<ProductOption> CreateProductOption(ProductOption productOption);
        Task UpdateProductOption(ProductOption productOption);
        Task DeleteProductOption(Guid productId, Guid id);
    }
}