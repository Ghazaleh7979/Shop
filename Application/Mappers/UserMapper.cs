using Domain.Dtos;
using Domain.Models;

namespace Application.Mappers;

public static class UserMapper
{
    public static UserInfo ToUserInfo(this User user) => new (user.Id, user.Email, user.Username,
        user.Products?.Select(product => product.ToProductDto()).ToList());
}