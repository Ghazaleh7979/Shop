using Domain.Dtos;

namespace Domain.Responses;

public sealed record GetProductsResponse(List<ProductInfo> ProductsInfo, int Total);