using Application.IRepositories;
using Application.Mappers;
using Domain.Dtos;
using Domain.Requests;
using Domain.Requests.User;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User;

public sealed class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserInfo> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.CreateUser(request, cancellationToken);
        return user.ToUserInfo();
    }
}