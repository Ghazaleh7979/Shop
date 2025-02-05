using Application.IRepositories;
using Application.UseCases.Product;
using Domain.Dtos;
using Domain.Models;
using Moq;

namespace Test
{
    public class GetProductUseCaseTest
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly GetProductUseCase _getProductUseCase;
        public GetProductUseCaseTest()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _getProductUseCase = new GetProductUseCase(_mockProductRepo.Object);
        }
        [Fact]
        public async void Get_ShouldReturnProductInfo_WhenProductExists()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var fakeProduct = new Product { Id = productId, Name = "test" };
            var expectedProductInfo = new ProductInfo(productId, "test", DateTime.Now, "", "", true, new UserInfo(Guid.NewGuid(), "", "", null));

            _mockProductRepo
                .Setup(repo => repo.GetProductById(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProduct);

            //Act
            var result = await _getProductUseCase.Get(productId, It.IsAny<CancellationToken>());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProductInfo.Id, result.Id);
            Assert.Equal(expectedProductInfo.Name, result.Name);
        }
    }
}
