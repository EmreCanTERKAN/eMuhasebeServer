using AutoMapper;
using eMuhasebeServer.Application.Features.Banks;
using eMuhasebeServer.Application.Features.CashRegisters;
using eMuhasebeServer.Application.Features.Companies;
using eMuhasebeServer.Application.Features.Customers;
using eMuhasebeServer.Application.Features.Invoices;
using eMuhasebeServer.Application.Features.Products;
using eMuhasebeServer.Application.Features.Users;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enums;

namespace eMuhasebeServer.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserCommand, AppUser>();
        CreateMap<UpdateUserCommand, AppUser>();

        CreateMap<CreateCompanyCommand, Company>();
        CreateMap<UpdateCompanyCommand, Company>();

        CreateMap<CreateCashRegisterCommand, CashRegister>().ForMember(member => member.CurrencyType, options =>
        {
            options.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
        });

        CreateMap<UpdateCashRegisterCommand, CashRegister>().ForMember(member => member.CurrencyType, options =>
        {
            options.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
        });

        CreateMap<CreateBankCommand, Bank>().ForMember(member => member.CurrencyType, options =>
        {
            options.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
        });

        CreateMap<UpdateBankCommand, Bank>().ForMember(member => member.CurrencyType, options =>
        {
            options.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
        });

        CreateMap<CreateCustomerCommand, Customer>().ForMember(member => member.Type, options =>
        {
            options.MapFrom(map => CustomerTypeEnum.FromValue(map.TypeValue));
        });
        CreateMap<UpdateCustomerCommand, Customer>().ForMember(member => member.Type, options =>
        {
            options.MapFrom(map => CustomerTypeEnum.FromValue(map.TypeValue));
        });

        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();

        CreateMap<CreateInvoiceCommand, Invoice>()
            .ForMember(member => member.Type, options =>
            {
                options.MapFrom(map => InvoiceTypeEnum.FromValue(map.TypeValue));
            })
            .ForMember(member => member.Details, options =>
            {
                options.MapFrom(map => map.Details.Select(s => new InvoiceDetail()
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    Price = s.Price
                }).ToList());
            })
            .ForMember(member => member.Amount, options =>
            {
                options.MapFrom(map => map.Details.Sum(s => s.Quantity * s.Price));
            });
    }
}
