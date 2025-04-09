using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products;
public sealed record UpdateProductCommand(
    Guid Id,
    string Name) : IRequest<Result<string>>;

internal sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<UpdateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id);

        if (product is null)
        {
            return Result<string>.Failure("Stok bulunamadı.");
        }

        bool isNameExists = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isNameExists)
        {
            return Result<string>.Failure("Stok adı daha önce kaydedilmiş.");
        }

        mapper.Map(request, product);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        cacheService.Remove("products");

        return "Stok güncellendi";
    }
}
