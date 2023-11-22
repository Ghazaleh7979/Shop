namespace Domain.Requests.Product;

public record ProductQueryParameter(int Skip, int Take, Guid? UserId);