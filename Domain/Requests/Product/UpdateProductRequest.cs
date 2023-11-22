using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.Product;

public record UpdateProductRequest(
    [Required(AllowEmptyStrings = false)] string Name,
    [Phone] string ManufacturePhone,
    [EmailAddress] string ManufactureEmail,
    bool IsAvailable);