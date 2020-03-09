using System;
using NSubstitute;
using XeroRefactor.Models;
using XeroRefactor.Repositories;
using XeroRefactor.Services;
using Xunit;

namespace XeroRefactorUnitTests
{
    public class ProductOptionServiceUnitTests
    {
        private readonly IProductOptionService _service;
        private readonly IProductOptionRepository _productOptionRepository;

        public ProductOptionServiceUnitTests()
        {
            _productOptionRepository = Substitute.For<IProductOptionRepository>();
            _service = new ProductOptionService(_productOptionRepository);
        }

        [Fact]
        public async void ProductOptionServiceGetAll_ShouldCall_ProductOptionRepositoryToReturnAllProducts()
        {
            //Arrange
            var productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

            //Act
            await _service.GetByProductIdAsync(productId);

            //Assert
            await _productOptionRepository.Received(1).GetProductOptions(productId);
        }

        [Fact]
        public async void ProductOptionServiceSearchByName_ShouldCall_ProductOptionRepositoryToReturnAllProductsContainsTheName()
        {
            //Arrange
            var productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");
            var id = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");

            //Act
            await _service.GetByProductOptionIdAsync(productId, id);

            //Assert
            await _productOptionRepository.Received(1).GetProductOption(productId, id);
        }



        [Fact]
        public async void ProductOptionServiceCreateAProduct_ShouldCall_ProductOptionRepositoryToCreateANewProductOption()
        {
            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");

            ProductOption model = new ProductOption
            {
                ProductId = productId,
                Name = "Red",
                Description = "Red Samsung Galaxy S7"
            };

            //Act
            await _service.AddAsync(productId, model);

            //Assert
            await _productOptionRepository.Received(1).CreateProductOption(model);
        }

        [Fact]
        public async void ProductOptionServiceUpdateAProduct_ShouldCall_ProductOptionRepositoryToUpdateAProduct()
        {
            //Arrange
            var productOptionId = Guid.Parse("a21d5777-a655-4020-b431-624bb331e9a2");

            ProductOption productOption = new ProductOption
            {
                Id = productOptionId,
                ProductId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                Name = "Grey",
                Description = "Grey Samsung Galaxy S7"
            };
            //Act
            await _service.UpdateAsync(productOptionId, productOption);

            //Assert
            await _productOptionRepository.Received(1).UpdateProductOption(productOption);
        }

        [Fact]
        public async void ProductOptionServiceDeleteAProduct_ShouldCall_ProductOptionRepositoryToDeleteAProduct()
        {
            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var id = Guid.Parse("a21d5777-a655-4020-b431-624bb331e9a2");
            //Act
            await _service.DeleteAsync(productId, id);

            //Assert
            await _productOptionRepository.Received(1).DeleteProductOption(productId, id);
        }


    }
}
