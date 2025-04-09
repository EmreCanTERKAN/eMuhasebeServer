using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies;
public sealed record GetAllCompanyQuery() : IRequest<Result<List<Company>>>;



internal sealed class GetAllCompanyQueryHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService) : IRequestHandler<GetAllCompanyQuery, Result<List<Company>>>
{
    public async Task<Result<List<Company>>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
    {

        List<Company>? componies;

        componies = cacheService.Get<List<Company>>("companies");

        if (componies is null)
        {
            componies = await companyRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            cacheService.Set<List<Company>>("companies", componies);
        }


        return componies;
    }
}
