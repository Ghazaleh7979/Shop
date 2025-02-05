using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;
using Domain.Requests.Product;

namespace Application.UseCases.Product;

public sealed class UpdateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public UpdateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductInfo> Update(Guid id, UpdateProductRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.UpdateProduct(id, request, cancellationToken);
        if (userId != product.UserId)
        {
            throw new Exception("You have not created this product !!");
        }
        return product.ToProductInfo();
    }


}