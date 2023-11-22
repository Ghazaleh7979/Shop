using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.User;

public record CreateUserRequest(
    [EmailAddress] string Email,
    string Username,
    string Password);