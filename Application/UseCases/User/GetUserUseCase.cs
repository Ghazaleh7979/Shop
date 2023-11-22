using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;

namespace Application.UseCases.User;

public sealed class GetUserUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserInfo> Get(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(id, cancellationToken);
        return user.ToUserInfo();
    }
}