namespace Domain.Models;

public sealed class User 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime InsertTime { get; set; } 
    public ICollection<Product>? Products { get; set; }
    public bool IsDeleted { get; set; }
}