using Application.IRepositories;
using Domain.Dtos;
using Domain.Requests;
using Application.Mappers;

namespace Application.UseCases;

public sealed class RemoveUserUseCase
{
    private readonly IUserRepository _userRepository;

    public RemoveUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUser(id, cancellationToken);
    }
}