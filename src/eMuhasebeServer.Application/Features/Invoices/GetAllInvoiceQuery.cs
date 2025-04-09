using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices;
public sealed record GetAllInvoiceQuery() : IRequest<Result<List<Invoice>>>;


internal sealed class GetAllInvoiceQueryHandler(
    IInvoiceRepository invoiceRepository,
    ICacheService cacheService) : IRequestHandler<GetAllInvoiceQuery, Result<List<Invoice>>>
{
    public async Task<Result<List<Invoice>>> Handle(GetAllInvoiceQuery request, CancellationToken cancellationToken)
    {
        List<Invoice>? invoices;

        string key = "invoices";

        invoices = cacheService.Get<List<Invoice>>(key);

        if (invoices is null)
        {
            invoices = await invoiceRepository
                .GetAll()
                .Include(p => p.Customer)
                .Include(p => p.Details!)
                .ThenInclude(p => p.Product)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);

            cacheService.Set(key, invoices);
        }

        return invoices;
    }
}
