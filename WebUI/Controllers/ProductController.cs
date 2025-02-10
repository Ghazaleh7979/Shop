using Application.UseCases;
using Application.UseCases.Product;
using Domain.Requests.Product;
using Domain.Responses;
using Domain.Responses.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Store.Domain.Responses;
using System.Security.Claims;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]")]

public class ProductController : ControllerBase
{
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly UpdateProductUseCase _updateProductUseCase;
    private readonly RemoveProductUseCase _removeProductUseCase;
    private readonly GetProductUseCase _getProductUseCase;
    private readonly GetAllProductsUseCase _getAllProductsUseCase;

    public ProductController(CreateProductUseCase createProductUseCase,
        UpdateProductUseCase updateProductUseCase,
        RemoveProductUseCase removeProductUseCase,
        GetProductUseCase getProductUseCase,
        GetAllProductsUseCase getAllProductsUseCase)
    {
        _createProductUseCase = createProductUseCase;
        _updateProductUseCase = updateProductUseCase;
        _removeProductUseCase = removeProductUseCase;
        _getProductUseCase = getProductUseCase;
        _getAllProductsUseCase = getAllProductsUseCase;
    }

    [HttpPost]
    //[Authorize]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        //var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var product = await _createProductUseCase.Create(request, Guid.Parse("a698bacb-bf3f-4800-8e99-746e68622ba1"), cancellationToken);
        return Ok(new CreateProductResponse(product));
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<CreateProductResponse>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var product = await _updateProductUseCase.Update(id, request, userId, cancellationToken);
        return Ok(new CreateProductResponse(product));
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        await _removeProductUseCase.Remove(id, userId, cancellationToken);
        return Ok();
    }

    [HttpGet("")]
    public async Task<ActionResult<GetProductsResponse>> GetProducts([FromQuery] ProductQueryParameter parameter,
        CancellationToken cancellationToken)
    {
        var (products, total) = await _getAllProductsUseCase.GetAll(parameter, cancellationToken);
        return Ok(new GetProductsResponse(products, total));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetProductResponse>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await _getProductUseCase.Get(id, cancellationToken);
        return Ok(new GetProductResponse(product));
    }
}