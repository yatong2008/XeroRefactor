using System;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.NSubstitute;
using NSubstitute;
using XeroRefactor.Data;
using XeroRefactor.Models;
using XeroRefactor.Repositories;
using Xunit;

namespace XeroRefactorUnitTests
{
    public class ProductOptionRepositoryUnitTests
    {

        private readonly IProductContext _context;
        private readonly IProductOptionRepository _productOptionRepository;

        public ProductOptionRepositoryUnitTests()
        {
            _context = Substitute.For<IProductContext>();
            _productOptionRepository = new ProductOptionRepository(_context);

            var productData = new List<Product>
            {
                new Product {
                    Id = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                    Name = "Samsung Galaxy S7",
                    Description = "Newest mobile product from Samsung.",
                    Price = 1024.99M,
                    DeliveryPrice = 16.99M
                },
                new Product {
                    Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Name = "Apple iPhone 6S",
                    Description = "Newest mobile product from Apple.",
                    Price = 1299.99M,
                    DeliveryPrice = 15.99M
                }
            }.AsQueryable();

            var mockProductSet = productData.AsQueryable().BuildMockDbSet();

            _context.Products.Returns(mockProductSet);

            var productOptionData = new List<ProductOption>
            {
                new ProductOption
                {
                    Id = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9"),
                    ProductId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                    Name = "White",
                    Description = "White Samsung Galaxy S7"
                },
                new ProductOption
                {
                    Id = Guid.Parse("9cbcf0e7-67a2-4a30-a805-c8ac19711618"),
                    ProductId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
                    Name = "Red",
                    Description = "Red Samsung Galaxy S7"
                },
                new ProductOption
                {
                    Id = Guid.Parse("5c2996ab-54ad-4999-92d2-89245682d534"),
                    ProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Name = "Rose Gold",
                    Description = "Gold Apple iPhone 6S"
                },
                new ProductOption
                {
                    Id = Guid.Parse("9ae6f477-a010-4ec9-b6a8-92a85d6c5f03"),
                    ProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Name = "White",
                    Description = "White Apple iPhone 6S"
                },
                new ProductOption
                {
                    Id = Guid.Parse("4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef"),
                    ProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                    Name = "Black",
                    Description = "Black Apple iPhone 6S"
                }
            };

            var mockProductOptionDataSet = productOptionData.AsQueryable().BuildMockDbSet();
            _context.ProductOptions.Returns(mockProductOptionDataSet);
        }

        [Fact]
        public async void RepositoryGetAllProductOptions_ShouldCall_ContextToReturnTheCorrectAllProductOptions()
        {

            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");


            //Act
            var results = await _productOptionRepository.GetProductOptions(productId);

            //Assert
            Assert.NotNull(results?.FirstOrDefault(x => x.Id == Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9")));
            Assert.NotNull(results?.FirstOrDefault(x => x.Id == Guid.Parse("9cbcf0e7-67a2-4a30-a805-c8ac19711618")));
        }


        [Fact]
        public async void RepositoryGetProductOption_ShouldCall_ContextToReturnTheCorrectProductOption()
        {

            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var id = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            

            //Act
            var result = await _productOptionRepository.GetProductOption(productId, id);

            //Assert
            //_context.ProductOptions?.Received(1).FirstOrDefault(x => x.ProductId == productId && x.Id == id);

            Assert.Equal(Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9"), result.Id);
            Assert.Equal(Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"), result.ProductId);
            Assert.Equal("White", result.Name);
            Assert.Equal("White Samsung Galaxy S7", result.Description);
        }

        [Fact]
        public async void RepositoryCreateProductOption_ShouldCall_ContextToCreateProductOption()
        {

            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");

            ProductOption productOption = new ProductOption
            {
                ProductId = productId,
                Name = "Red",
                Description = "Red Samsung Galaxy S7"
            };

            //Act
            await _productOptionRepository.CreateProductOption(productOption);

            //Assert
            _context.ProductOptions.Received(1).Add(Arg.Is<ProductOption>(x => x.ProductId == productId));

        }

        [Fact]
        public async void RepositoryUpdateProductOption_ShouldCall_ContextToUpdateProductOption()
        {

            //Arrange
            var id = Guid.Parse("4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef");

            ProductOption productOption = new ProductOption
            {
                Id = id,
                ProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                Name = "Space Grey",
                Description = "Space Grey Black Apple iPhone 6S"
            };

            //Act
            await _productOptionRepository.UpdateProductOption(productOption);
            var result = _context.ProductOptions.FirstOrDefault(x => x.Id == id);

            //Assert
            _context.ProductOptions.Received(1).Update(Arg.Is<ProductOption>(x => x.Id == id 
                                                                                  && x.Name == productOption.Name
                                                                                  && x.Description == productOption.Description));

        }

        [Fact]
        public async void RepositoryDeleteProductOption_ShouldCall_ContextToDeleteProductOption()
        {

            //Arrange
            var productId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");
            var id = Guid.Parse("4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef");

            //Act
            await _productOptionRepository.DeleteProductOption(productId, id);

            //Assert
            _context.ProductOptions.Received(1).Remove(Arg.Is<ProductOption>(x => x.Id == id && x.ProductId == productId));
        }
    }
}
