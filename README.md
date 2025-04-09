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


![Açıklama](https://github.com/EmreCanTERKAN/eMuhasebeServer/blob/master/docs/images/auth.png)