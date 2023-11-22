using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.User;

public record UpdateUserRequest(
    [EmailAddress] string Email,
    string Username);