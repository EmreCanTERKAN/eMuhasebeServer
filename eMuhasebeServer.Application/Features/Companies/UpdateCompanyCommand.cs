using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Domain.ValueObjects;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies;
public sealed record UpdateCompanyCommand (
    Guid Id,
    string Name,
    string TaxDepartment,
    string TaxNumber,
    string FullAddress,
    Database Database) : IRequest<Result<string>>;

public sealed class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Şirket Adı Boş olamaz");
        RuleFor(p => p.FullAddress).NotEmpty().WithMessage("Adres alanı boş olamaz");
        RuleFor(p => p.Database.DatabaseName).NotEmpty().WithMessage("Veritabanı alanı boş olamaz");
        RuleFor(p => p.Database.Server).NotEmpty().WithMessage("Sunucu alanı boş olamaz");

    }
}

internal sealed class UpdateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateCompanyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await companyRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (company is null)
        {
            return Result<string>.Failure("Şirket bulunamadı");
        }

        if(company.TaxNumber != request.TaxNumber)
        {
            bool isNameExist = await companyRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);

            if (isNameExist)
            {
                return Result<string>.Failure("Bu vergi numarasına ait şirket mevcut");
            }
        }

        mapper.Map(request, company);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şirket başarıyla güncellendi";
    }
}
