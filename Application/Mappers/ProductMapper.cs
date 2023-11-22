using Domain.Dtos;
using Domain.Models;

namespace Application.Mappers;

public static class ProductMapper
{
    public static ProductInfo ToProductInfo(this Product product) => new (product.Id, product.Name, product.ProduceDate,
        product.ManufacturePhone, product.ManufactureEmail, product.IsAvailable, product.User!.ToUserInfo());
    
    public static ProductDto ToProductDto(this Product product) => new (product.Id, product.Name, product.ProduceDate,
        product.ManufacturePhone, product.ManufactureEmail, product.IsAvailable);
}