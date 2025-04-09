using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products;
public sealed record CreateProductCommand(
    string Name) : IRequest<Result<string>>;

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICacheService cacheService) : IRequestHandler<CreateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        bool isNameExists = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isNameExists)
        {
            return Result<string>.Failure("Bu stok daha önce kaydedilmiş");
        }
        Product product = mapper.Map<Product>(request);

        await productRepository.AddAsync(product, cancellationToken);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("products");

        return "Stok kaydedildi";


    }
}
