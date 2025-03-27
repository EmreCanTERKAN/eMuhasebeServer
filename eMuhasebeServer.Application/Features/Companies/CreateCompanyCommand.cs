using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Domain.ValueObjects;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies;
public sealed record CreateCompanyCommand(
    string Name,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber,
    Database Database) : IRequest<Result<string>>;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Şirket Adı Boş olamaz")
            .MinimumLength(3).WithMessage("Şirket Adı En Az 3 Karakter olmalı.");
        RuleFor(p => p.FullAddress)
            .NotEmpty().WithMessage("Adres alanı boş olamaz")
            .MinimumLength(3).WithMessage("Adres alanı En Az 3 Karakter olmalı.");
        RuleFor(p => p.Database.DatabaseName)
            .NotEmpty().WithMessage("Veritabanı alanı boş olamaz")
            .MinimumLength(3).WithMessage("Veritabanı alanı En Az 3 Karakter olmalı.");
        RuleFor(p => p.Database.Server)
            .NotEmpty().WithMessage("Sunucu alanı boş olamaz")
            .MinimumLength(3).WithMessage("Sunucu alanı En Az 3 Karakter olmalı.");
        RuleFor(p => p.TaxDepartment)
            .NotEmpty().WithMessage("Vergi dairesi ismi boş olamaz")
            .MinimumLength(3).WithMessage("Vergi dairesi En Az 3 Karakter olmalı.");
        RuleFor(p => p.TaxNumber)
            .NotEmpty().WithMessage("Vergi numarası boş olamaz")
            .MinimumLength(3).WithMessage("Vergi numarası En Az 3 Karakter olmalı.");

    }
}

internal sealed class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateCompanyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        bool isNameExist = await companyRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);

        if (isNameExist)
        {
            return Result<string>.Failure("Bu vergi numarasına ait şirket mevcut");
        }

        Company company = mapper.Map<Company>(request);

        await companyRepository.AddAsync(company, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Şirket başarıyla oluşturuldu";
    }
}
