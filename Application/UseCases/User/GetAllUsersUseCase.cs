using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;
using Domain.Requests;
using Domain.Requests.User;

namespace Application.UseCases.User;

public sealed class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(List<UserInfo>, int)> GetAll(UserQueryParameter parameter, CancellationToken cancellationToken)
    {
        var (user, total) = await _userRepository.GetUsers(parameter, cancellationToken);
        return (user.Select(user1 => user1.ToUserInfo()).ToList(), total);
    }
}