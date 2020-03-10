using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XeroRefactor.Data;
using XeroRefactor.Exceptions;
using XeroRefactor.Models;

namespace XeroRefactor.Repositories
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IProductContext _context;

        public ProductOptionRepository(IProductContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId)
        {
            if (!ProductExists(productId))
            {
                throw new ObjectNotFoundException($"Product with id '{productId}' not found");
            }
            return await _context.ProductOptions
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductOption> GetProductOption(Guid productId, Guid id)
        {
            if (!ProductExists(productId))
            {
                throw new ObjectNotFoundException($"Product with id '{productId}' not found");
            }
            return await _context.ProductOptions.FirstOrDefaultAsync(x => x.ProductId == productId && x.Id == id);
        }

        public async Task<ProductOption> CreateProductOption(ProductOption productOption)
        {
            if (!ProductExists(productOption.ProductId))
            {
                throw new ObjectNotFoundException($"Product with id '{productOption.ProductId}' not found");
            }
            _context.ProductOptions.Add(productOption);
            await _context.SaveChangesAsync();
            return productOption;
        }

        public async Task UpdateProductOption(ProductOption productOption)
        {
            if (!ProductExists(productOption.ProductId))
            {
                throw new ObjectNotFoundException($"Product with id '{productOption.ProductId}' not found");
            }
            if (!ProductOptionExists(productOption.Id))
            {
                throw new ObjectNotFoundException($"ProductOption with id '{productOption.ProductId}' not found");
            }
            
            _context.ProductOptions.Update(productOption);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductOption(Guid productId, Guid id)
        {
            if (ProductOptionExists(id))
            {
                ProductOption productOption = _context.ProductOptions.FirstOrDefault(x => x.Id == id);
                if (productOption != null && (productOption.ProductId == productId || !ProductExists(productOption.ProductId)))
                {
                    _context.ProductOptions.Remove(productOption);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ObjectNotFoundException($"ProductOption with id '{id}' not found");
            }
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private bool ProductOptionExists(Guid id)
        {
            return _context.ProductOptions.Any(e => e.Id == id);
        }
    }
}
