using Application.IRepositories;
using Domain.Requests;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Login;

public sealed class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<Domain.Models.User> _passwordHasher;

    public LoginUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<Domain.Models.User>();
    }

    public async Task<bool> CheckLogin(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsername(request.Username, cancellationToken);
        if (user == null) return false;

        return user.PasswordHash == request.Password;
    }
}