namespace Domain.Dtos;

public sealed record ProductDto(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable
    );