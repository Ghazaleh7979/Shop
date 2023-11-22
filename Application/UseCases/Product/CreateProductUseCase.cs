using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;
using Domain.Requests;
using Domain.Requests.Product;

namespace Application.UseCases.Product;

public sealed class CreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public CreateProductUseCase(IProductRepository productRepository,
        IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<ProductInfo> Create(CreateProductRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.CreateProduct(request, userId, cancellationToken);
        return product.ToProductInfo();
    }
}