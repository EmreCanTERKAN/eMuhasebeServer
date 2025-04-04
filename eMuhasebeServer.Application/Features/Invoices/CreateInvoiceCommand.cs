﻿using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enums;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices;
public sealed record CreateInvoiceCommand(
    int TypeValue,
    DateOnly Date,
    string InvoiceNumber,
    Guid CustomerId,
    List<InvoiceDetailDto> InvoiceDetails) :IRequest<Result<string>>;

internal sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IProductRepository productRepository,
    IProductDetailRepository productDetailRepository,
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<CreateInvoiceCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        #region Fature ve Detay

        Invoice invoice = mapper.Map<Invoice>(request);

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        #endregion
        #region Customer
        Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.CustomerId, cancellationToken);

        if(customer is null)
        {
            return Result<string>.Failure("Müşteri bulunamadı");
        }

        customer.DepositAmount += request.TypeValue == 2 ? invoice.Amount : 0;
        customer.WithdrawalAmount += request.TypeValue == 1 ? invoice.Amount : 0;


        CustomerDetail customerDetail = new()
        {
            CustomerId = customer.Id,
            Date = request.Date,
            DepositAmount = request.TypeValue == 2 ? invoice.Amount : 0,
            WithdrawalAmount = request.TypeValue == 1 ? invoice.Amount : 0,
            Description = invoice.InvoiceNumber + " Numaralı " + invoice.Type.Name,
            Type = request.TypeValue == 1 ? CustomerDetailTypeEnum.PurchaseInvoiece : CustomerDetailTypeEnum.SellingInvoice
        };

        await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
        #endregion
        #region Product
        foreach (var item in request.InvoiceDetails)
        {
            Product product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == item.ProductId, cancellationToken);

            product.Deposit += request.TypeValue == 1 ? item.Quantity : 0;
            product.Withdrawal += request.TypeValue == 2 ? item.Quantity : 0;

            ProductDetail productDetail = new()
            {
                ProductId = product.Id,
                Date = request.Date,
                Description = invoice.InvoiceNumber + " Numaralı " + invoice.Type.Name,
                Deposit = request.TypeValue == 1 ? item.Quantity : 0,
                Withdrawal = request.TypeValue == 2 ? item.Quantity : 0
            };

            await productRepository.AddAsync(product, cancellationToken);
            await productDetailRepository.AddAsync(productDetail, cancellationToken);
        }       
        #endregion
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("purchaseInvoices");
        cacheService.Remove("sellingInvoices");
        cacheService.Remove("customers");
        cacheService.Remove("products");

        return invoice.Type.Name + "Fatura kaydı başarıyla tamamlandı";

    }
}
