using Domain.Dtos;

namespace Domain.Responses.User;

public sealed record GetUsersResponse(List<UserInfo> UserInfo, int Total);