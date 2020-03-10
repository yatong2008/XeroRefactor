using System;
using NSubstitute;
using XeroRefactor.Models;
using XeroRefactor.Repositories;
using XeroRefactor.Services;
using Xunit;

namespace XeroRefactorUnitTests.ServiceUnitTests
{
    public class ProductServiceUnitTests
    {
        private readonly IProductService _service;
        private readonly IProductRepository _productRepository;

        public ProductServiceUnitTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _service = new ProductService(_productRepository);
        }

        [Fact]
        public async void ProductServiceGetAll_ShouldCall_ProductRepositoryToReturnAllProducts()
        {
            //Arrange

            //Act
            await _service.GetAllAsync();

            //Assert
            await _productRepository.Received(1).GetProducts();
        }

        [Fact]
        public async void ProductServiceSearchByName_ShouldCall_ProductRepositoryToReturnAllProductsContainsTheName()
        {
            //Arrange
            var searchKeyWord = "Samsung";

            //Act
            await _service.GetAllByNameAsync(searchKeyWord);

            //Assert
            await _productRepository.Received(1).SearchProductsByName(searchKeyWord);
        }

        [Fact]
        public async void ProductServiceGetAProductById_ShouldCall_ProductRepositoryToGetTheProductById()
        {
            //Arrange
            var productId = Guid.Parse("0F3F7EC9-BA7A-4DAD-BD8D-5310DF26AE63");

            //Act
            await _service.GetByIdAsync(productId);

            //Assert
            await _productRepository.Received(1).GetByIdAsync(productId);
        }

        [Fact]
        public async void ProductServiceCreateAProduct_ShouldCall_ProductRepositoryToCreateANewProduct()
        {
            //Arrange
            Product model = new Product
            {
                Name = "Samsung Galaxy S20 Ultra",
                Description = "Say hello to a whole new way of seeing. Galaxy S20 Ultra 5G features a 108MP camera, 100x zoom lens, the world’s first smartphone with 8K video recording at 24fps, advanced design and seriously fast 5G.",
                Price = 1999.99M,
                DeliveryPrice = 19.99M
            };

            //Act
            await _service.CreateAsync(model);

            //Assert
            await _productRepository.Received(1).CreateProductAsync(model);
        }

        [Fact]
        public async void ProductServiceUpdateAProduct_ShouldCall_ProductRepositoryToUpdateAProduct()
        {
            //Arrange
            Product model = new Product
            {
                Id = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                Name = "Samsung Galaxy S7",
                Description = "Best 2017 mobile product from Samsung.",
                Price = 999.99M,
                DeliveryPrice = 9.99M
            };
            //Act
            await _service.UpdateAsync(model);

            //Assert
            await _productRepository.Received(1).UpdateProduct(model);
        }

        [Fact]
        public async void ProductServiceDeleteAProduct_ShouldCall_ProductRepositoryToDeleteAProduct()
        {
            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");

            //Act
            await _service.DeleteAsync(productId);

            //Assert
            await _productRepository.Received(1).DeleteProduct(productId);
        }

    }
}
