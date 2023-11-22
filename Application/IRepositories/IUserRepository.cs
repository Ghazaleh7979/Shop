using Domain.Models;
using Domain.Requests;
using Domain.Requests.User;

namespace Application.IRepositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsername(string userName, CancellationToken cancellationToken);
    Task<User> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    Task<User> UpdateUser(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task DeleteUser(Guid id, CancellationToken cancellationToken);
    Task<(List<User>, int)> GetUsers(UserQueryParameter parameter, CancellationToken cancellationToken);
    Task<User> GetUserById(Guid id, CancellationToken cancellationToken);

}