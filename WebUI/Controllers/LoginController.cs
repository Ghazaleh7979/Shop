using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.IRepositories;
using Application.Mappers;
using Application.UseCases.Login;
using Domain.Dtos;
using Domain.Requests;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebUI.Controllers;

public class LoginController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public LoginController(
        LoginUseCase loginUseCase,
        IConfiguration configuration,
        IUserRepository userRepository)
    {
        _loginUseCase = loginUseCase;
        _configuration = configuration;
        _userRepository = userRepository;
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        if (await _loginUseCase.CheckLogin(request, cancellationToken))
        {
            var user = await _userRepository.GetUserByUsername(request.Username, cancellationToken);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWT:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim("UserId", user!.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["JWT:ExpirationHour"]!)),
                signingCredentials: signIn);

            return Ok(new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token),
                new UserInfo(user.Id, user.Email, user.Username,
                    user.Products?.Select(product => product.ToProductDto()).ToList())));
        }

        return Unauthorized();
    }
}