using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;
using Domain.Requests.Product;

namespace Application.UseCases.Product;

public sealed class CreateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductInfo> Create(CreateProductRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.CreateProduct(request, userId, cancellationToken);
        return product.ToProductInfo();
    }
}