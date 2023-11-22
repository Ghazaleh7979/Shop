using Domain.Dtos;

namespace Domain.Responses;

public sealed record LoginResponse(string Token, UserInfo User
    //, List<Permission> Permissions
    );