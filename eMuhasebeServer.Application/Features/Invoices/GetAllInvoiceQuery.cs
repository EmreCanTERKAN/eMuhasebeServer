using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eMuhasebeServer.Application.Features.Invoices;
public sealed record GetAllInvoiceQuery(
    int Type) : IRequest<List<Invoice>>;


internal sealed class GetAllInvoiceQueryHandler(
    IInvoiceRepository invoiceRepository,
    ICacheService cacheService) : IRequestHandler<GetAllInvoiceQuery, List<Invoice>>
{
    public async Task<List<Invoice>> Handle(GetAllInvoiceQuery request, CancellationToken cancellationToken)
    {
        List<Invoice>? invoices;
        string key = "";
        if (request.Type == 1)
        {
            key = "purchaseInvoices";
        }
        else
        {
            key = "sellingInvoices";
        }

        invoices = cacheService.Get<List<Invoice>>(key);

        if (invoices is null)
        {
            invoices = await invoiceRepository
                .Where(p => p.Type.Value == request.Type)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);

            cacheService.Set(key, invoices);
        }

        return invoices;
    }
}
