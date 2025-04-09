
# Muhasebe Yönetim Sistemi

Bu proje, .NET 9.0 ile geliştirilmiş, çoklu şirket desteğine sahip, ölçeklenebilir bir şirket yönetim uygulamasıdır. Uygulama, Clean Architecture ve CQRS gibi modern yazılım mimarileri ile inşa edilmiştir. Her şirketin kendine ait bağımsız veritabanı bulunur ve sistem bu yapıya uygun olarak genişletilebilir şekilde tasarlanmıştır.

## Genel Özellikler

-   Clean Architecture yapısı kullanılmıştır.
    
-   CQRS (Command & Query Responsibility Segregation) prensibine göre ayrılmış iş mantığı.
    
-   IdentityServer üzerinden kimlik doğrulama ve login işlemleri.
    
-   Her şirket için ayrı bir veritabanı (multi-tenancy desteği).
    
-   Şirketler, bankalar, müşteriler, ürünler, kasalar ve bunlara ait detayların yönetimi.
    
-   Her yapının kendi endpoint’i mevcuttur.
    
-   Stoklar üzerinden yapılan kârlılık analizleri ve detaylı raporlama.
    
-   Alış ve satış faturalarının yönetimi.
    
-   Satış faturaları, ana ekranda grafiksel rapor olarak gösterilir. Fatura eklenip silindiğinde veriler SignalR ile canlı olarak güncellenir.
    

## Kullanılan Kütüphane ve Araçlar

Projede aşağıdaki kütüphaneler ve desenler kullanılmıştır:

-   **SignalR** – Gerçek zamanlı bildirimler için
    
-   **MediatR** – CQRS yapısının uygulanması için
    
-   **FluentValidation** – Model doğrulama işlemleri için
    
-   **AutoMapper** – Nesne dönüşümleri için
    
-   **Scrutor** – Servislerin otomatik olarak IOC container’a eklenmesi
    
-   **FluentEmail** – SMTP üzerinden e-posta gönderimi
    
-   **Result Pattern** – Katmanlar arası standart dönüş yapısı
    
-   **Unit of Work** & **Generic Repository Pattern** – Veritabanı işlemlerinin daha kontrollü ve sürdürülebilir hale getirilmesi için
    

## E-Posta Gönderimi

Kullanıcı kayıt işlemi sonrasında, yapılandırılmış bir SMTP sunucusu üzerinden kullanıcıya otomatik olarak bilgilendirme e-postası gönderilir. Bu işlem FluentEmail paketiyle gerçekleştirilmiştir.

## Geliştirme Notları

-   Kod yapısı test edilebilirlik ve genişletilebilirlik ön planda tutularak tasarlanmıştır.
    
-   Katmanlar arası bağımlılık en aza indirilmiş, servis ve veri erişim yapıları soyutlanmıştır.
    
-   Uygulama üzerinde geliştirmeye açık birçok modül ve alan bulunmaktadır.
## API istekleri

Aşağıda, uygulamanın çeşitli modüllerine ait API endpoint'lerinin ekran görüntüleri ve açıklamaları yer almaktadır.

### **Authentication (Auth)**

Giriş ve kullanıcı doğrulama işlemleri ile ilgili API'nin ekran görüntüsü:

![Authentication](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/auth.png)

### **Banks (Bankalar)**

Bankalar modülünün API'si ile ilgili ekran görüntüsü:

![Banks](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/banks.png)

### **Companies (Şirketler)**

Şirketlerin yönetildiği API endpoint'leri ve ekran görüntüsü:

![Companies](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/companies.png)

### **Customers (Müşteriler)**

Müşteri yönetimi ile ilgili API ekran görüntüsü:

![Customers](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/customers.png)

### **Products (Ürünler)**

Ürünler yönetimi API'si ve ilgili ekran görüntüsü:

![Products](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/products.png)

### **Reports (Raporlar)**

Stok karlılığı ve finansal raporların API endpoint'leri:

![Reports](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/reports.png)

### **Users (Kullanıcılar)**

Kullanıcı yönetimi API'si ve ekran görüntüsü:

![Reports](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/users.png)

### **CashRegisters (Kasalar)**

Kasa yönetimi API'si ve ekran görüntüsü:

![Reports](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/users.png)

### **Folders**
```
└── 📁src
    └── 📁eMuhasebeServer.Application
        └── 📁Behaviors
            └── ValidationBehavior.cs
        └── DependencyInjection.cs
        └── eMuhasebeServer.Application.csproj
        └── 📁Features
            └── 📁Auth
                └── 📁ChangeCompany
                    └── ChangeCompanyCommand.cs
                └── 📁ConfirmEmail
                    └── ConfirmEmailCommand.cs
                └── 📁Login
                    └── LoginCommand.cs
                    └── LoginCommandHandler.cs
                    └── LoginCommandResponse.cs
                    └── LoginCommandValidator.cs
                └── 📁SendConfirmEmail
                    └── SendConfirmEmailCommand.cs
            └── 📁BankDetails
                └── CreateBankDetailCommand.cs
                └── DeleteBankDetailByIdCommand.cs
                └── GetAllBankDetailsQuery.cs
                └── UpdateBankDetailCommand.cs
            └── 📁Banks
                └── CreateBankCommand.cs
                └── DeleteBankByIdCommand.cs
                └── GetAllBanksQuery.cs
                └── UpdateBankCommand.cs
            └── 📁CashRegisterDetails
                └── CreateCashRegisterDetailCommand.cs
                └── DeleteCashRegisterDetailByIdCommand.cs
                └── GetAllCashRegisterDetailsQuery.cs
                └── UpdateCashRegisterDetailCommand.cs
            └── 📁CashRegisters
                └── CreateCashRegisterCommand.cs
                └── DeleteCashRegisterByIdCommand.cs
                └── GetAllCashRegistersQuery.cs
                └── UpdateCashRegisterCommand.cs
            └── 📁Companies
                └── CreateCompanyCommand.cs
                └── DeleteCompanyByIdCommand.cs
                └── GetAllCompanyQuery.cs
                └── MigrateAllCompaniesCommand.cs
                └── UpdateCompanyCommand.cs
            └── 📁CustomerDetails
                └── GetAllCustomerDetailQuery.cs
            └── 📁Customers
                └── CreateCustomerCommand.cs
                └── DeleteCustomerByIdCommand.cs
                └── GetAllCustomersQuery.cs
                └── UpdateCustomerCommand.cs
            └── 📁Invoices
                └── CreateInvoiceCommand.cs
                └── DeleteInvoiceByIdCommand.cs
                └── GetAllInvoiceQuery.cs
            └── 📁ProductDetails
                └── GetAllProductDetailQuery.cs
            └── 📁Products
                └── CreateProductCommand.cs
                └── DeleteProductByIdCommand.cs
                └── GetAllProductQuery.cs
                └── UpdateProductCommand.cs
            └── 📁Reports
                └── ProductProfitabilityReportsQuery.cs
                └── PurchaseReportsQuery.cs
            └── 📁Users
                └── CreateUserCommand.cs
                └── DeleteUserByIdCommand.cs
                └── GetAllUsersQuery.cs
                └── UpdateUserCommand.cs
        └── 📁Hubs
            └── ReportHub.cs
        └── 📁Mapping
            └── MappingProfile.cs
        └── 📁Services
            └── ICacheService.cs
            └── ICompanyService.cs
            └── IJwtProvider.cs
    └── 📁eMuhasebeServer.Domain
        └── 📁Abstractions
            └── Entity.cs
        └── 📁Dtos
            └── InvoiceDetailDto.cs
        └── eMuhasebeServer.Domain.csproj
        └── 📁Entities
            └── AppUser.cs
            └── Bank.cs
            └── BankDetail.cs
            └── CashRegister.cs
            └── CashRegisterDetail.cs
            └── Company.cs
            └── CompanyUser.cs
            └── Customer.cs
            └── CustomerDetail.cs
            └── Invoice.cs
            └── InvoiceDetail.cs
            └── Product.cs
            └── ProductDetail.cs
        └── 📁Enums
            └── CurrencyTypeEnum.cs
            └── CustomerDetailTypeEnum.cs
            └── CustomerTypeEnum.cs
            └── InvoiceTypeEnum.cs
        └── 📁Events
            └── AppUserEvent.cs
            └── SendConfirmEmailEvent.cs
        └── 📁Repositories
            └── IBankDetailRepository.cs
            └── IBankRepository.cs
            └── ICashRegisterDetailRepository.cs
            └── ICashRegisterRepository.cs
            └── ICompanyRepository.cs
            └── ICompanyUserRepository.cs
            └── ICustomerDetailRepository.cs
            └── ICustomerRepository.cs
            └── IInvoiceDetailRepository.cs
            └── IInvoiceRepository.cs
            └── IProductDetailRepository.cs
            └── IProductRepository.cs
            └── IUnitOfWorkCompany.cs
            └── IUnitOfWorkForClearTracking.cs
        └── 📁ValueObjects
            └── Database.cs
    └── 📁eMuhasebeServer.Infrastructure
        └── 📁Configurations
            └── AppUserConfiguration.cs
            └── CompanyConfiguration.cs
            └── CompanyUserConfiguration.cs
        └── 📁Context
            └── ApplicationDbContext.cs
            └── CompanyDbContext.cs
        └── DependencyInjection.cs
        └── eMuhasebeServer.Infrastructure.csproj
        └── 📁Migrations
            └── 📁CompanyDb
        └── 📁Options
            └── JwtOptions.cs
            └── JwtTokenOptionsSetup.cs
        └── 📁Repositories
            └── BankDetailRepository.cs
            └── BankRepository.cs
            └── CashRegisterDetailRepository.cs
            └── CashRegisterRepository.cs
            └── CompanyRepository.cs
            └── CompanyUserRepository.cs
            └── CustomerDetailRepository.cs
            └── CustomerRepository.cs
            └── InvoiceDetailRepository.cs
            └── InvoiceRepository.cs
            └── ProductDetailRepository.cs
            └── ProductRepository.cs
        └── 📁Services
            └── CompanyService.cs
            └── JwtProvider.cs
            └── MemoryCacheService.cs
            └── RedisCacheService.cs
            └── UnitOfWorkForClearTracking.cs
    └── 📁eMuhasebeServer.WebAPI
        └── 📁Abstractions
            └── ApiController.cs
        └── appsettings.Development.json
        └── appsettings.json
        └── 📁Controllers
            └── AuthController.cs
            └── BankDetailsController.cs
            └── BanksController.cs
            └── CashRegisterDetailsController.cs
            └── CashRegistersController.cs
            └── CompaniesController.cs
            └── CustomerDetailsController.cs
            └── CustomersController.cs
            └── InvoicesController.cs
            └── ProductsController.cs
            └── ReportsController.cs
            └── SeedDataController.cs
            └── TestController.cs
            └── UsersController.cs
        └── eMuhasebeServer.WebAPI.csproj
        └── 📁Middlewares
            └── ExceptionHandler.cs
            └── ExtensionsMiddleware.cs
        └── Program.cs
        └── 📁Properties
            └── launchSettings.json
        └── WebAPI.http
```
