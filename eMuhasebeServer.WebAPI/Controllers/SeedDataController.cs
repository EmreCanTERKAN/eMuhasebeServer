using Bogus;
using eMuhasebeServer.Application.Features.Customers;
using eMuhasebeServer.Application.Features.Invoices;
using eMuhasebeServer.Application.Features.Products;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Enums;
using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace eMuhasebeServer.WebAPI.Controllers;

public sealed class SeedDataController : ApiController
{

    public SeedDataController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        Faker faker = new();
        Random random = new Random();

        ////Customers
        //for (int i = 0; i < 1000; i++)
        //{
        //    int customerTypeValue = random.Next(1, 5);


        //    CreateCustomerCommand customer = new(
        //        faker.Company.CompanyName(),
        //        CustomerTypeEnum.FromValue(customerTypeValue),
        //        faker.Address.City(),
        //        faker.Address.State(),
        //        faker.Address.FullAddress(),
        //        faker.Company.Random.String2(random.Next(10, 25)),
        //        faker.Company.Random.ULong(1111111111, 99999999999).ToString());

        //    await _mediator.Send(customer);
        //}

        ////Products
        //for (int i = 0; i < 10000; i++)
        //{
        //    CreateProductCommand product = new(
        //        faker.Commerce.ProductName());

        //    await _mediator.Send(product);
        //}

        //Invoices

        var customerResult = await _mediator.Send(new GetAllCustomersQuery());

        var customers = customerResult.Data;

        var productResult = await _mediator.Send(new GetAllProductQuery());

        var products = productResult.Data;
        for (int i = 0; i < 100; i++)
        {
            if (products is null) continue;
            if (customers is null) continue;

            List<InvoiceDetailDto> invoiceDetails = new();

            for (int j = 0; j < random.Next(1, 11); j++)
            {
                InvoiceDetailDto invoiceDetailDto = new()
                {
                    
                    ProductId = products[random.Next(1, products.Count)].Id,
                    Price = random.Next(1, 1000),
                    Quantity = random.Next(1, 100)
                };

                invoiceDetails.Add(invoiceDetailDto);
            }

            CreateInvoiceCommand invoice = new(
                random.Next(1, 3),
                new DateOnly(2025, random.Next(1, 13), random.Next(1, 29)),
                faker.Random.ULong(3, 16).ToString(),
                customers[random.Next(1, customers.Count)].Id,
                invoiceDetails);

            await _mediator.Send(invoice);

        }

        return Ok(Result<string>.Succeed("Seed Data başarıyla oluşturuldu"));



    }
}
