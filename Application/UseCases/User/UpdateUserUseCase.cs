using Application.IRepositories;
using Domain.Dtos;
using Domain.Requests;
using Application.Mappers;
using Domain.Requests.User;

namespace Application.UseCases;

public sealed class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserInfo> Create(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.UpdateUser(id, request, cancellationToken);
        return user.ToUserInfo();
    }
}