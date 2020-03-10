using System;
using NSubstitute;
using XeroRefactor.Controllers;
using XeroRefactor.Models;
using XeroRefactor.Services;
using Xunit;

namespace XeroRefactorUnitTests.ControllerUnitTests
{
    public class ProductControllerUnitTests
    {
        private readonly IProductService _productService;
        private readonly IProductOptionService _productOptionService;
        private readonly ProductsController _controller;

        public ProductControllerUnitTests()
        {
            _productService = Substitute.For<IProductService>();
            _productOptionService = Substitute.For<IProductOptionService>();

            _controller = new ProductsController(_productService, _productOptionService);

        }

        [Fact]
        public async void ProductsControllerReceiveAGetProductRequest_Should_CallServiceToReturnAllProducts()
        {
            // Arrange

            // Act
            var result = await _controller.Get(null);
            // Assert
            await _productService.Received(1).GetAllAsync();
        }

        [Fact]
        public async void ProductsControllerReceiveASearchProductRequest_Should_CallServiceToSearchProductsWithTheKeyWord()
        {
            // Arrange

            // Act
            var result = await _controller.Get("Samsung");
            // Assert
            await _productService.Received(1).GetAllByNameAsync("Samsung");
        }

        [Fact]
        public async void ProductsControllerReceiveGetAProductRequest_Should_CallServiceToGetProductById()
        {
            // Arrange
            var productId = Guid.Parse("0F3F7EC9-BA7A-4DAD-BD8D-5310DF26AE63");

            // Act
            var result = await _controller.Get(productId);
            // Assert
            await _productService.Received(1).GetByIdAsync(productId);
        }

        [Fact]
        public async void ProductsControllerReceiveAPostProductRequest_Should_CallServiceToSaveTheProduct()
        {
            // Arrange
            Product product = new Product
            {
                Name = "Samsung Galaxy S20 Ultra",
                Description = "Say hello to a whole new way of seeing. Galaxy S20 Ultra 5G features a 108MP camera, 100x zoom lens, the world’s first smartphone with 8K video recording at 24fps, advanced design and seriously fast 5G.",
                Price = 1999.99M,
                DeliveryPrice = 19.99M
            };
            // Act
            var result = await _controller.Post(product);
            // Assert
            await _productService.Received(1).CreateAsync(Arg.Is<Product>(x => x.Name == product.Name));
        }

        [Fact]
        public async void ProductsControllerReceiveAUpdateProductRequest_Should_CallServiceToUpdateTheProduct()
        {
            // Arrange
            Product product = new Product
            {
                Id = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                Name = "Samsung Galaxy S7",
                Description = "Best 2017 mobile product from Samsung.",
                Price = 999.99M,
                DeliveryPrice = 9.99M
            };
            // Act
            var result = await _controller.Update(product.Id, product);
            // Assert
            await _productService.Received(1).UpdateAsync(Arg.Is<Product>(x => x.Id == product.Id));
        }

        [Fact]
        public async void ProductsControllerReceiveADeleteProductRequest_Should_CallServiceToDeleteTheProduct()
        {
            // Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            // Act
            var result = await _controller.Delete(productId);
            // Assert
            await _productService.Received(1).DeleteAsync(productId);
        }




        [Fact]
        public async void ProductsControllerReceiveARequestProductOptionsRequest_Should_CallServiceToGetAllProductOptionsRelatedToAProduct()
        {
            // Arrange
            var productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");
            // Act
            var result = await _controller.GetOptions(productId);
            // Assert
            await _productOptionService.Received(1).GetByProductIdAsync(productId);
        }


        [Fact]
        public async void ProductsControllerReceiveGetAProductOptionRequest_Should_CallServiceToGetProductOptionById()
        {
            // Arrange
            var productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");
            var id = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            // Act
            var result = await _controller.GetOption(productId, id);
            // Assert
            await _productOptionService.Received(1).GetByProductOptionIdAsync(productId, id);
        }

        [Fact]
        public async void ProductsControllerReceiveAPostProductOptionRequest_Should_CallServiceToSaveTheProductOption()
        {
            // Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");

            ProductOption productOption = new ProductOption
            {
                ProductId = productId,
                Name = "Red",
                Description = "Red Samsung Galaxy S7"
            };
            // Act
            var result = await _controller.CreateOption(productId, productOption);
            // Assert
            await _productOptionService.Received(1).AddAsync(productId, Arg.Is<ProductOption>(x => x.ProductId == productId));
        }

        [Fact]
        public async void ProductsControllerReceiveAUpdateProductOptionRequest_Should_CallServiceToUpdateTheProductOption()
        {
            // Arrange
            var productOptionId = Guid.Parse("a21d5777-a655-4020-b431-624bb331e9a2");

            ProductOption productOption = new ProductOption
            {
                Id = productOptionId,
                ProductId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                Name = "Grey",
                Description = "Grey Samsung Galaxy S7"
            };
            // Act
            var result = await _controller.UpdateOption(productOptionId, productOption);
            // Assert
            await _productOptionService.Received(1).UpdateAsync(productOptionId, Arg.Is<ProductOption>(x => x.Id == productOptionId));
        }

        [Fact]
        public async void ProductsControllerReceiveADeleteProductOptionRequest_Should_CallServiceToDeleteTheProductOption()
        {
            // Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var id = Guid.Parse("a21d5777-a655-4020-b431-624bb331e9a2");
            // Act
            var result = await _controller.DeleteOption(productId, id);
            // Assert
            await _productOptionService.Received(1).DeleteAsync(productId, id);
        }

    }
}
