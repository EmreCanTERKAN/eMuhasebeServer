using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks;
public sealed record CreateBankCommand(
    string Name,
    string IBAN,
    int CurrencyTypeValue) : IRequest<Result<string>>;

internal sealed class CreateBankCommandHandler(
    IBankRepository bankRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<CreateBankCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        bool isIbanExist = await bankRepository.AnyAsync(p => p.IBAN == request.IBAN, cancellationToken);

        if (isIbanExist)
        {
            return Result<string>.Failure("Iban daha önce kadedilmiş");
        }

        Bank bank = mapper.Map<Bank>(request);

        await bankRepository.AddAsync(bank, cancellationToken);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");

        return "Banka başarıyla kaydedildi";
    }
}
