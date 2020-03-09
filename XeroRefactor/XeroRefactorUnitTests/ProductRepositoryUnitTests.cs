using System;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.NSubstitute;
using NSubstitute;
using XeroRefactor.Data;
using XeroRefactor.Exceptions;
using XeroRefactor.Models;
using XeroRefactor.Repositories;
using Xunit;

namespace XeroRefactorUnitTests
{
    public class ProductRepositoryUnitTests
    {
        private readonly IProductContext _context;
        private readonly IProductRepository _productRepository;


        public ProductRepositoryUnitTests()
        {
            _context = Substitute.For<IProductContext>();
            _productRepository = new ProductRepository(_context);

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
        public async void GetProductById_Should_ReceiveAFindProductCall()
        {
            //Arrange
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");

            //Act
            var result = await _productRepository.GetByIdAsync(productId);
            
            //Assert
            _context.Products.Received(1).FirstOrDefault(x => x.Id == productId);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async void SearchProduct_Should_ReceiveCorrectResults()
        {
            //Arrange
            var keyword = "Samsung";

            //Act
            var results = await _productRepository.SearchProductsByName(keyword);

            //Assert

            foreach (var result in results)
            {
                Assert.True(result.Name.Contains(keyword));
            }
        }

        [Fact]
        public async void AddNewProduct_Should_ReceiveANewProduct()
        {
            //Arrange
            var newProduct = new Product
            {
                Id = Guid.Parse("8f2e9a76-35ee-4f0a-ae55-83023d2db1a3"),
                Name = "Samsung Galaxy S20",
                Description = "Newest mobile product from Samsung.",
                Price = 1999.99M,
                DeliveryPrice = 9.99M
            };

            //Act
            await _productRepository.CreateProductAsync(newProduct);

            //Assert
            await _context.Received(1).AddAsync(Arg.Is<Product>(x => x.Id == newProduct.Id
                                                               && x.Name == newProduct.Name
                                                               && x.Description == newProduct.Description
                                                               && x.Price == newProduct.Price
                                                               && x.DeliveryPrice == newProduct.DeliveryPrice));

        }

        [Fact]
        public async void DeleteProduct_Should_ReceiveARemoveProductCall()
        {
            //Arrange
            var id = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var productOptionId1 = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            var productOptionId2 = Guid.Parse("9cbcf0e7-67a2-4a30-a805-c8ac19711618");

            //Act
            await _productRepository.DeleteProduct(id);

            //Assert
            _context.Products.Received(1).Remove(Arg.Is<Product>(x => x.Id == id));

            _context.ProductOptions.Remove(Arg.Is<ProductOption>(x => x.Id == productOptionId1));
            _context.ProductOptions.Remove(Arg.Is<ProductOption>(x => x.Id == productOptionId2));
        }

        [Fact]
        public async void ProductServiceDeleteAProductWithAnInvalidId_ShouldCall_ThrowAnObjectNotFoundException()
        {
            //Arrange
            var productId = Guid.Parse("00000000-0000-0000-0000-000000000000");

            //Act

            //Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(async () => await _productRepository.DeleteProduct(productId));

        }


        [Fact]
        public async void UpdateProduct_Should_ReceiveAUpdateProductCall()
        {
            //Arrange
            var id = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            
            Product model = new Product
            {
                Id = id,
                Name = "Samsung Galaxy S7",
                Description = "Best 2017 mobile product from Samsung.",
                Price = 999.99M,
                DeliveryPrice = 9.99M
            };

            //Act
            await _productRepository.UpdateProduct(model);
            var result = _context.ProductOptions.FirstOrDefault(x => x.Id == id);

            //Assert
            _context.Products.Received(1).Update(Arg.Is<Product>(x => x.Id == id
                                                                                  && x.Name == model.Name
                                                                                  && x.Description == model.Description
                                                                                  && x.Price == model.Price
                                                                                  && x.DeliveryPrice == model.DeliveryPrice));

        }




    }
}
