using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Events;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users;
public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    List<Guid> CompanyIds) : IRequest<Result<string>>;

internal sealed class CreateUserCommandHandler(
    IMediator mediator,
    UserManager<AppUser> userManager,
    ICompanyUserRepository companyUserRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isUserNameExists = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);

        if (isUserNameExists)
        {
            return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış");
        }

        bool isEmailExists = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);

        if (isEmailExists)
        {
            return Result<string>.Failure("Bu mail adresi daha önce kullanılmış");
        }

        AppUser appUser = mapper.Map<AppUser>(request);

        IdentityResult identityResult = await userManager.CreateAsync(appUser, request.Password);

        if (!identityResult.Succeeded)
        {
            return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
        }

        List<CompanyUser> companyUsers = request.CompanyIds.Select(p => new CompanyUser()
        {
            CompanyId = p,
            AppUserId = appUser.Id
        }).ToList();

        await companyUserRepository.AddRangeAsync(companyUsers, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("users");

        await mediator.Publish(new AppUserEvent(appUser.Id));

        return "Kullanıcı kaydı başarıyla tamamlandı";
    }
}
