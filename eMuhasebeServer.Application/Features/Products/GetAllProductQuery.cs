using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products;
public sealed record GetAllProductQuery () : IRequest<Result<List<Product>>>;

internal sealed class GetAllProductQueryHandler(
    IProductRepository productRepository,
    ICacheService cacheService) : IRequestHandler<GetAllProductQuery, Result<List<Product>>>
{
    public async Task<Result<List<Product>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        List<Product>? products;
        products = cacheService.Get<List<Product>>("products");

        if (products is null)
        {
            products = await productRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            cacheService.Set("products",products);
        }
        return products;

    }
}
