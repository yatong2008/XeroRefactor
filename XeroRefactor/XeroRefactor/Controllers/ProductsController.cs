using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeroRefactor.Models;
using XeroRefactor.Services;

namespace XeroRefactor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductOptionService _productOptionService;

        public ProductsController(IProductService productEfService, IProductOptionService productOptionService)
        {
            _productService = productEfService;
            _productOptionService = productOptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Ok(new Products
                    {
                        Items = await _productService.GetAllAsync()
                    });
                }

                return Ok(new Products
                {
                    Items = await _productService.GetAllByNameAsync(name)
                });
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            var result = await _productService.CreateAsync(product);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Product product)
        {
            await _productService.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetOptions(Guid productId)
        {
            try
            {
                var productOptions = new ProductOptions
                {
                    Items = await _productOptionService.GetByProductIdAsync(productId)
                };
                return Ok(productOptions);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<IActionResult> GetOption(Guid productId, Guid id)
        {
            try
            {
                var productOption = await _productOptionService.GetByProductOptionIdAsync(productId, id);
                return Ok(productOption);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateOption(Guid productId, ProductOption option)
        {
            try
            {
                await _productOptionService.AddAsync(productId, option);
                return Ok(option);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{productId}/options/{id}")]
        public async Task<IActionResult> UpdateOption(Guid id, ProductOption option)
        {
            try
            {
                await _productOptionService.UpdateAsync(id, option);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{productId}/options/{id}")]
        public async Task<IActionResult> DeleteOption(Guid productId, Guid id)
        {
            try
            {
                await _productOptionService.DeleteAsync(productId, id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}