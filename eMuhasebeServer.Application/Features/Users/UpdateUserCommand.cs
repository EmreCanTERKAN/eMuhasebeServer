using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Events;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users;
public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Password,
    List<Guid> CompanyIds) : IRequest<Result<string>>;


internal sealed class UpdateUsercommandHandler(
    IMediator mediator,
    UserManager<AppUser> userManager,
    ICompanyUserRepository companyUserRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.Users
            .Where(p => p.Id == request.Id)
            .Include(p => p.CompanyUsers)
            .FirstOrDefaultAsync(cancellationToken);

        bool isMailChanged = false;

        if (appUser is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        if (appUser.UserName != request.UserName || appUser.Email != request.Email)
        {
            var existingUser = await userManager.Users
                .Where(p => p.UserName == request.UserName || p.Email == request.Email)
                .Select(p => new { p.UserName, p.Email })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser is not null)
            {
                if (existingUser.UserName == request.UserName && existingUser.Email == request.Email)
                    return Result<string>.Failure("Bu kullanıcı adı ve email daha önce kayıt edilmiştir.");

                return existingUser.UserName == request.UserName
                    ? Result<string>.Failure("Bu kullanıcı adı daha önce kayıt edilmiştir.")
                    : Result<string>.Failure("Bu Email daha önce kayıt edilmiştir.");
            }

            isMailChanged = true;
            appUser.EmailConfirmed = false;
        }

        mapper.Map(request, appUser);

        IdentityResult identityResult = await userManager.UpdateAsync(appUser);

        if (!identityResult.Succeeded)
        {
            return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
        }

        if (request.Password is not null)
        {
            string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

            identityResult = await userManager.ResetPasswordAsync(appUser, token, request.Password);

            if (!identityResult.Succeeded)
            {
                return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
            }
        }

        companyUserRepository.DeleteRange(appUser.CompanyUsers);

        List<CompanyUser> companyUsers = request.CompanyIds.Select(p => new CompanyUser
        {
            AppUserId = appUser.Id,
            CompanyId = p
        }).ToList();

        await companyUserRepository.AddRangeAsync(companyUsers,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (isMailChanged)
        {
            await mediator.Publish(new AppUserEvent(appUser.Id));
        }

        return "Kullanıcı başarıyla güncellendi";

    }
}
