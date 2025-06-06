﻿using eMuhasebeServer.Application.Hubs;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices;
public sealed record DeleteInvoiceByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteInvoiceByIdCommandHandler(
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IProductRepository productRepository,
    IProductDetailRepository productDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IUnitOfWorkForClearTracking unitOfWorkForClearTracking,
    IHubContext<ReportHub> hubContext,
    ICacheService cacheService) : IRequestHandler<DeleteInvoiceByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteInvoiceByIdCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await invoiceRepository.Where(p => p.Id == request.Id).Include(p => p.Details).FirstOrDefaultAsync(cancellationToken);

        if (invoice is null)
        {
            return Result<string>.Failure("Fatura bulunamadı");
        }

        CustomerDetail? customerDetail = await customerDetailRepository.Where(p => p.InvoiceId == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (customerDetail is not null)
        {
            customerDetailRepository.Delete(customerDetail);
        }

        Customer? customer = await customerRepository.Where(p => p.Id == invoice.CustomerId).FirstOrDefaultAsync(cancellationToken);

        if (customer is not null)
        {
            customer.DepositAmount -= invoice.Type.Value == 1 ? 0 : invoice.Amount;
            customer.WithdrawalAmount -= invoice.Type.Value == 2 ? 0 : invoice.Amount;

            customerRepository.Update(customer);
        }

        List<ProductDetail> productDetails = await productDetailRepository.Where(p => p.InvoiceId ==
        invoice.Id).ToListAsync(cancellationToken);

        foreach (var detail in productDetails)
        {
            Product? product = await productRepository.GetByExpressionAsync(p => p.Id == detail.ProductId, cancellationToken);

            if (product is not null)
            {
                product.Deposit -= detail.Deposit;
                product.Withdrawal -= detail.Withdrawal;

                productRepository.Update(product);
            }
        }

        invoiceRepository.Delete(invoice);
        productDetailRepository.DeleteRange(productDetails);


        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        unitOfWorkForClearTracking.ClearTracking();

        cacheService.Remove("invoices");
        cacheService.Remove("customers");
        cacheService.Remove("products");

        await hubContext.Clients.All.SendAsync("PurchaseReports", new { Date = invoice.Date, Amount = invoice.Amount, Action = "delete" });


        return invoice.Type.Name + " kaydı başarıyla silindi";
    }
}
