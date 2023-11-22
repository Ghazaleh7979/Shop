using Application.IRepositories;
using Domain.Models;
using Domain.Requests.User;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BasicDatabase _database;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public UserRepository(BasicDatabase database, IServiceScopeFactory serviceScopeFactory)
    {
        _database = database;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task<User?> GetUserByUsername(string userName, CancellationToken cancellationToken)
    {
        return _database.Users!
            .FirstOrDefaultAsync(user => user.Username.ToLower() == userName.ToLower(), cancellationToken);
    }

    public async Task<User> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        return await _database.Users!
            .Include(user => user.Products)
            .FirstAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<User> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            Username = request.Username,
            InsertTime = DateTime.Now,
            Products = new List<Product>(),
            PasswordHash = request.Password,
            IsDeleted = false
        };

        await _database.Users!.AddAsync(user, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        
        return await GetUserById(user.Id, cancellationToken);
    }

    public async Task<User> UpdateUser(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await GetUserById(id, cancellationToken);

        user.Username = request.Username;
        user.Email = request.Email;

        await _database.SaveChangesAsync(cancellationToken);
        
        return await GetUserById(user.Id, cancellationToken);
    }

    public async Task<(List<User>, int)> GetUsers(UserQueryParameter parameter, CancellationToken cancellationToken)
    {
        var queryableUser = _database.Users!
            .Include(user => user.Products)
            .AsQueryable();
        
        if (parameter.Skip != 0) queryableUser = queryableUser.Skip(parameter.Skip);
        if (parameter.Take != 0) queryableUser = queryableUser.Take(parameter.Take);
        
        queryableUser = queryableUser.OrderBy(user => user.InsertTime);

        var total = await queryableUser.CountAsync(cancellationToken);
        return (await queryableUser.ToListAsync(cancellationToken), total);
    }

    public async Task DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetUserById(id, cancellationToken);

        user.IsDeleted = true;

        await _database.SaveChangesAsync(cancellationToken);
    }
}