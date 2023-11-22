using Application.IRepositories;
using Domain.Dtos;
using Domain.Requests;
using Application.Mappers;

namespace Application.UseCases;

public sealed class RemoveProductUseCase
{
    private readonly IProductRepository _productRepository;

    public RemoveProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Remove(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(id, cancellationToken);
        if (userId != product.UserId)
        {
            throw new Exception(" You have not created this product !!");
        }
        await _productRepository.DeleteProduct(id, cancellationToken);
    }
}