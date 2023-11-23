using Application.UseCases;
using Application.UseCases.User;
using Domain.Requests.User;
using Domain.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly UpdateUserUseCase _updateUserUseCase;
    private readonly RemoveUserUseCase _removeUserUseCase;
    private readonly GetUserUseCase _getUserUseCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;

    public UserController(CreateUserUseCase createUserUseCase,
        UpdateUserUseCase updateUserUseCase,
        RemoveUserUseCase removeUserUseCase,
        GetUserUseCase getUserUseCase,
        GetAllUsersUseCase getAllUsersUseCase)
    {
        _createUserUseCase = createUserUseCase;
        _updateUserUseCase = updateUserUseCase;
        _removeUserUseCase = removeUserUseCase;
        _getUserUseCase = getUserUseCase;
        _getAllUsersUseCase = getAllUsersUseCase;
    }

    [HttpPost("")]
    public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _createUserUseCase.Create(request, cancellationToken);
        return Ok(new CreateUserResponse(user));
    }
    
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<CreateUserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _updateUserUseCase.Create(id, request, cancellationToken);
        return Ok(new CreateUserResponse(user));
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteUsers(Guid id, CancellationToken cancellationToken)
    {
        await _removeUserUseCase.Remove(id, cancellationToken);
        return Ok();
    }
    
    [HttpGet("")]
    [Authorize]
    public async Task<ActionResult<GetUsersResponse>> GetUsers([FromQuery] UserQueryParameter parameter,
        CancellationToken cancellationToken)
    {
        var (users, total) = await _getAllUsersUseCase.GetAll(parameter, cancellationToken);
        return Ok(new GetUsersResponse(users, total));
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<GetUserResponse>> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _getUserUseCase.Get(id, cancellationToken);
        return Ok(new GetUserResponse(user));
    }
}