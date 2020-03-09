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
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new ObjectNotFoundException($"Product with id '{id}' not found");
            }
            return product;
        }

        public async Task<List<Product>> SearchProductsByName(string name)
        {
            return await _context.Products.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);

                foreach (var productOption in await _context.ProductOptions.Where(x => x.ProductId == id).ToListAsync())
                {
                    _context.ProductOptions.Remove(productOption);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ObjectNotFoundException($"Product with id '{id}' not found");

            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProduct(Product product)
        {
            if (!ProductExists(product.Id))
            {
                throw new ObjectNotFoundException($"Product with id '{product.Id}' not found");
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
