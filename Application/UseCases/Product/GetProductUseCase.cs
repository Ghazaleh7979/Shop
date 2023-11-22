using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;

namespace Application.UseCases.Product;

public sealed class GetProductUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductInfo> Get(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(id, cancellationToken);
        return product.ToProductInfo();
    }
}