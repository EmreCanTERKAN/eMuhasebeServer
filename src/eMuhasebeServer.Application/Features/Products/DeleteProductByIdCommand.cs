using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products;
public sealed record DeleteProductByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteProductByIdCommandHandler(
    IProductRepository productRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<DeleteProductByIdCommand, Result<string>> 
{                                                                              
    public async Task<Result<string>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        Product product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            return Result<string>.Failure("Stok bulunamadı");
        }

        product.IsDeleted = true;
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("products");

        return "Stok silindi";


    }
}
