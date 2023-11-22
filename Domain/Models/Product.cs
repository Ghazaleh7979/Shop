namespace Domain.Models;

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; } = null!;
    public string ManufactureEmail { get; set; } = null!;
    public bool IsAvailable { get; set; }
    public bool IsDeleted { get; set; }
    public User? User { get; set; }
    public Guid UserId { get; set; }
}