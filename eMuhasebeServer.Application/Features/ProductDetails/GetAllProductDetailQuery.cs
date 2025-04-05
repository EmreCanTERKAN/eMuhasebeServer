using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

public sealed record GetAllProductDetailQuery(
    Guid ProductId) : IRequest<Result<Product>>;


internal sealed class GetAllProductDetailQueryHandler(
    IProductRepository productRepository) : IRequestHandler<GetAllProductDetailQuery, Result<Product>>
{
    public async Task<Result<Product>> Handle(GetAllProductDetailQuery request, CancellationToken cancellationToken)
    {
        Product? product = await productRepository
            .Where(p => p.Id == request.ProductId)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return Result<Product>.Failure("Stok bulunamadı");
        }
        return product;
    }
}
