using System.ComponentModel.DataAnnotations;

namespace Domain.Requests;

public sealed record LoginRequest(
    [Required(AllowEmptyStrings = false)] string Username,
    [Required(AllowEmptyStrings = false)] string Password
);