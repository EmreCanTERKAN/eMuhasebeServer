using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails;
public sealed record DeleteCashRegisterDetailByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteCashRegisterDetailByIdCommandHandler(
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<DeleteCashRegisterDetailByIdCommand, Result<string>>
{
    async Task<Result<string>> IRequestHandler<DeleteCashRegisterDetailByIdCommand, Result<string>>.Handle(DeleteCashRegisterDetailByIdCommand request, CancellationToken cancellationToken)
    {
        CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (cashRegisterDetail is null)
        {
            return Result<string>.Failure("Kasa hareketi bulunamadı");
        }

        CashRegister? cashRegister = await cashRegisterRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.Id, cancellationToken);

        if (cashRegister is null)
        {
            return Result<string>.Failure("Kasa Bulunamadı");
        }

        cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
        cashRegister.WithdrawalAmount -= cashRegisterDetail.WithdrawalAmount;

        if (cashRegisterDetail.CashRegisterDetailId is not null)
        {
            CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CashRegisterDetailId, cancellationToken);

            if (oppositeCashRegisterDetail is null)
            {
                return Result<string>.Failure("Kasa hareketi bulunamadı");
            }

            CashRegister? oppositeCashRegister = await cashRegisterRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == oppositeCashRegisterDetail.CashRegisterDetailId, cancellationToken);

            if (oppositeCashRegister is null)
            {
                return Result<string>.Failure("Kasa Bulunamadı");
            }

            oppositeCashRegister.DepositAmount -= oppositeCashRegisterDetail.DepositAmount;
            oppositeCashRegister.WithdrawalAmount -= oppositeCashRegisterDetail.WithdrawalAmount;
        }

        cashRegisterDetailRepository.Delete(cashRegisterDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return "Kasa hareketi başarıyla silindi";
    }
}
