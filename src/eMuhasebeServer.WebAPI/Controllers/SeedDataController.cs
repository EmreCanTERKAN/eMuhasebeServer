﻿using Bogus;
using eMuhasebeServer.Application.Features.Customers;
using eMuhasebeServer.Application.Features.Invoices;
using eMuhasebeServer.Application.Features.Products;
using eMuhasebeServer.Domain.Dtos;
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
        Random random = new Random();

        //Customers
        //for (int i = 0; i < 1000; i++)
        //{
        //    Faker faker = new();
        //    int customerTypeValue = random.Next(1, 5);
        //    CreateCustomerCommand customer = new(
        //        faker.Company.CompanyName(),
        //        customerTypeValue,
        //        faker.Address.City(),
        //        faker.Address.State(),
        //        faker.Address.FullAddress(),
        //        faker.Company.Random.String2(random.Next(10, 25)),
        //        faker.Company.Random.ULong(1111111111, 99999999999).ToString());

        //    await _mediator.Send(customer);
        //}

        //Products
        //for (int i = 0; i < 10000; i++)
        //{
        //    Faker faker = new();
        //    CreateProductCommand product = new(
        //        faker.Commerce.ProductName());

        //    await _mediator.Send(product);
        //}

        //Invoices
        var customersResult = await _mediator.Send(new GetAllCustomersQuery());
        var customers = customersResult.Data;

        var productsResult = await _mediator.Send(new GetAllProductQuery());
        var products = productsResult.Data!.Where(p => p.Withdrawal > 0).ToList();
        for (int i = 0; i < 100; i++)
        {
            Faker faker = new();

            if (products is null) continue;
            if (customers is null) continue;

            List<InvoiceDetailDto> invoiceDetails = new();
            for (int x = 0; x < random.Next(1, 11); x++)
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
                1,
                new DateOnly(2024, random.Next(1, 13), random.Next(1, 29)),
                faker.Random.ULong(3, 16).ToString(),
                customers[random.Next(1, customers.Count)].Id,
                invoiceDetails
                );

            await _mediator.Send(invoice);
        }

        return Ok(Result<string>.Succeed("Seed data başarıyla oluşturuldu"));
    }
}