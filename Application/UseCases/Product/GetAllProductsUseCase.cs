using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;
using Domain.Requests;
using Domain.Requests.Product;

namespace Application.UseCases.Product;

public sealed class GetAllProductsUseCase
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<(List<ProductInfo>, int)> GetAll(ProductQueryParameter parameter, CancellationToken cancellationToken)
    {
        var (product, total) = await _productRepository.GetProducts(parameter, cancellationToken);
        return (product.Select(product1 => product1.ToProductInfo()).ToList(), total);
    }
}