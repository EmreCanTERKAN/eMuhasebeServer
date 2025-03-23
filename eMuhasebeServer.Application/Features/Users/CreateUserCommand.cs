using AutoMapper;
using eMuhasebeServer.Domain.Entities;
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
    string Password) : IRequest<Result<string>>;

internal sealed class CreateUserCommandHandler(
    UserManager<AppUser> userManager,
    IMapper mapper) : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.Users
            .Where(p => p.UserName == request.UserName || p.Email == request.Email)
            .Select(p => new { p.UserName, p.Email })
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
        {
            var errors = new List<string>();

            if (existingUser.UserName == request.UserName)
                errors.Add("Bu kullanıcı adı daha önce kayıt edilmiştir.");

            if (existingUser.Email == request.Email)
                errors.Add("Bu Email daha önce kayıt edilmiştir.");

            return Result<string>.Failure(string.Join(" ", errors));
        }

        AppUser appUser = mapper.Map<AppUser>(request);

        IdentityResult identityResult = await userManager.CreateAsync(appUser, request.Password);

        if (identityResult.Succeeded)
        {
            return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
        }

        //Todo : onay maili gönderme

        return "Kullanıcı kaydı başarıyla tamamlandı";
    }
}
