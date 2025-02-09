using Application.IRepositories;
using Application.UseCases.Product;
using Domain.Dtos;
using Domain.Models;
using Domain.Requests.Product;
using Moq;

namespace TestShop
{
    public class CreateProductUseCaseTest
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly CreateProductUseCase _createProductUseCase;

        public CreateProductUseCaseTest()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _createProductUseCase = new CreateProductUseCase(_mockProductRepo.Object);
        }

        [Fact]
        public async Task Create_ShouldCreateNewProduct()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var fakeProduct = new Product { Id = productId, Name = "test" };
            var expectedProductInfo = new ProductInfo(productId, "test", DateTime.Now, "", "", true, new UserInfo(Guid.NewGuid(), "", "", null));

            _mockProductRepo
                .Setup(repo => repo.CreateProduct(new CreateProductRequest("test", "", "", true), Guid.NewGuid(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProduct);

            //Act
            var result = await _createProductUseCase.Create(new CreateProductRequest("test", "", "", true), Guid.NewGuid(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProductInfo.Id, result.Id);
            Assert.Equal(expectedProductInfo.Name, result.Name);
        }
    }
}
