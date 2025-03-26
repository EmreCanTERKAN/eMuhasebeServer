using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies;
public sealed record DeleteCompanyByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteCompanyByIdCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCompanyByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCompanyByIdCommand request, CancellationToken cancellationToken)
    {
        Company company = await companyRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (company is null)
        {
            return Result<string>.Failure("Şirket bulunamadı");
        }

        company.IsDeleted = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şirket başarıyla silindi";
    }
}
