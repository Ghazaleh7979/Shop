namespace Domain.Dtos;

public sealed record ProductInfo(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable,
    UserInfo User
    );