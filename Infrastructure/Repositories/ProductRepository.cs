using Application.IRepositories;
using Domain.Models;
using Domain.Requests.Product;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly BasicDatabase _database;
    private readonly IConnectionMultiplexer _redis;

    public ProductRepository(BasicDatabase database, IConnectionMultiplexer redis)
    {
        _database = database;
        _redis = redis;
    }

    public async Task<Product> CreateProduct(CreateProductRequest request, Guid userId,
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            ProduceDate = DateTime.Now,
            ManufacturePhone = request.ManufacturePhone,
            ManufactureEmail = request.ManufactureEmail,
            IsAvailable = request.IsAvailable,
            UserId = userId,
            IsDeleted = false
        };

        await _database.Products!.AddAsync(product, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);

        return await GetProductById(product.Id, cancellationToken);
    }

    public async Task<Product> UpdateProduct(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await GetProductById(id, cancellationToken);

        product.Name = request.Name;
        product.ManufactureEmail = request.ManufactureEmail;
        product.ManufacturePhone = request.ManufacturePhone;
        product.IsAvailable = request.IsAvailable;

        await _database.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        return await _database.Products!
            .Include(product => product.User)
            .Where(product => product.Id == id)
            .FirstAsync(cancellationToken);
    }

    public async Task<(List<Product>, int)> GetProducts(ProductQueryParameter parameter,
        CancellationToken cancellationToken)
    {
        #region Redis
        var db = _redis.GetDatabase();
        string key = $"Products : User : {parameter.UserId},{parameter.Take},{parameter.Skip}";

        var cachedProduct = await db.StringGetAsync(key);
        if (cachedProduct.HasValue)
        {
            var response = JsonConvert.DeserializeObject<List<Product>>(cachedProduct);
            return (response, response.Count);
        }
        #endregion

        var queryableProduct = _database.Products!
            .Include(product => product.User)
            .AsQueryable();

        if (parameter.UserId != null)
            queryableProduct =
                queryableProduct.Where(product => product.UserId == parameter.UserId);

        if (parameter.Skip != 0) queryableProduct = queryableProduct.Skip(parameter.Skip);
        if (parameter.Take != 0) queryableProduct = queryableProduct.Take(parameter.Take);

        queryableProduct = queryableProduct.OrderBy(product => product.ProduceDate);
        var products = await queryableProduct.ToListAsync(cancellationToken);
        string json = JsonConvert.SerializeObject(products, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        await db.StringSetAsync(key, json);
        var total = await queryableProduct.CountAsync(cancellationToken);
        return (products, total);
    }

    public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await GetProductById(id, cancellationToken);

        product.IsDeleted = true;

        await _database.SaveChangesAsync(cancellationToken);
    }
}