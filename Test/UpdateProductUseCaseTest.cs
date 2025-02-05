using Application.IRepositories;
using Application.UseCases.Product;
using Domain.Dtos;
using Domain.Models;
using Domain.Requests.Product;
using Moq;

namespace Test
{
    public class UpdateProductUseCaseTest
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly UpdateProductUseCase _updateProductUseCase;

        public UpdateProductUseCaseTest()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _updateProductUseCase = new UpdateProductUseCase(_mockProductRepo.Object);
        }
        [Fact]
        public async Task Update_ShouldUpdateProductData_WhenProductExists()
        {
            //Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var date = DateTime.Now;
            var fakeProduct = new Product
            {
                Id = id,
                Name = "test",
                ManufactureEmail = "1",
                ManufacturePhone = "1",
                IsAvailable = true,
                UserId = userId,
                ProduceDate = date
            };
            var exceptedProductInfo = new ProductInfo(id, "test1", date, "2", "2", false, new UserInfo(userId, "", "", null));

            _mockProductRepo
                .Setup(repo => repo.UpdateProduct(id, It.IsAny<UpdateProductRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProduct);

            //Act
            var result = await _updateProductUseCase.Update(id, new UpdateProductRequest("test1", "2", "2", false), userId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ProductInfo>(result);
            Assert.Equal(exceptedProductInfo.Id, result.Id);
            Assert.Equal(exceptedProductInfo.Name, result.Name);
            Assert.Equal(exceptedProductInfo.ManufactureEmail, result.ManufactureEmail);
            Assert.Equal(exceptedProductInfo.ManufacturePhone, result.ManufacturePhone);
            Assert.Equal(userId, result.User.UserId);
            Assert.Equal(exceptedProductInfo.IsAvailable, result.IsAvailable);
        }
        [Fact]
        public async Task Update_ShouldThrowException_WhenUserIdIsNotEqual()
        {
            var userId = Guid.NewGuid();
            var anotherUserId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var fakeProduct = new Product { Id = id, Name = "test", UserId = anotherUserId };


            _mockProductRepo
                .Setup(repo => repo.UpdateProduct(id, It.IsAny<UpdateProductRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeProduct);

            //Act , Assert
            await Assert.ThrowsAsync<Exception>(() => _updateProductUseCase.Update(id, new UpdateProductRequest("test1", "2", "2", false), userId, CancellationToken.None));
        }
    }
}
