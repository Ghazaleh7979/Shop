namespace Domain.Dtos;

public sealed record UserInfo(
    Guid UserId,
    string Email,
    string Username,
    List<ProductDto>? Products);