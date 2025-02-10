using Application.IRepositories;
using Application.UseCases;
using Application.UseCases.Login;
using Application.UseCases.Product;
using Application.UseCases.User;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BasicDatabase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var redisConnection = builder.Configuration["Redis:ConnectionString"];
var redis = ConnectionMultiplexer.Connect(redisConnection);
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

#region Repositories

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

#region UseCases

builder.Services.AddScoped<LoginUseCase>();

builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddScoped<RemoveProductUseCase>();
builder.Services.AddScoped<GetAllProductsUseCase>();
builder.Services.AddScoped<GetProductUseCase>();

builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<RemoveUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserUseCase>();

#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Shop",
            Description = "Shop",
            Contact = new OpenApiContact
            {
                Name = "Masoomeh Aliyannejhadi",
                Email = "masoome.aliyannejhadi@gmail.com",
            }
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//cache with redis
app.MapGet("/", async ([FromServices] IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    await db.StringSetAsync("testKey", "Hello Redis!");
    var value = await db.StringGetAsync("testKey");
    return Results.Ok(value.ToString());
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();