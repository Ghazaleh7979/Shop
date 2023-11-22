using Domain.Models;
using Domain.Requests;
using Domain.Requests.Product;

namespace Application.IRepositories;

public interface IProductRepository
{
    Task<Product> CreateProduct(CreateProductRequest request, Guid userId, CancellationToken cancellationToken);
    Task<Product> UpdateProduct(Guid id, UpdateProductRequest request, CancellationToken cancellationToken);
    Task DeleteProduct(Guid id, CancellationToken cancellationToken);
    Task<(List<Product>, int)> GetProducts(ProductQueryParameter parameter, CancellationToken cancellationToken);
    Task<Product> GetProductById(Guid id, CancellationToken cancellationToken);
}