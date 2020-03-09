using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XeroRefactor.Models;

namespace XeroRefactor.Services
{
    public interface IProductOptionService
    {
        Task<IEnumerable<ProductOption>> GetByProductIdAsync(Guid productId);
        Task<ProductOption> GetByProductOptionIdAsync(Guid productId, Guid id);
        Task<ProductOption> AddAsync(Guid productId, ProductOption model);
        Task UpdateAsync(Guid id, ProductOption model);
        Task DeleteAsync(Guid productId, Guid id);
    }
}
