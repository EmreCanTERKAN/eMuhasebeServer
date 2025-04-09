using Ardalis.SmartEnum;

namespace eMuhasebeServer.Domain.Enums;
public sealed class InvoiceTypeEnum : SmartEnum<InvoiceTypeEnum>
{
    public static readonly InvoiceTypeEnum Purchase = new InvoiceTypeEnum("Alış Faturası", 1);
    public static readonly InvoiceTypeEnum Selling = new InvoiceTypeEnum("Satış Faturası", 2);
    public InvoiceTypeEnum(string name, int value) : base(name, value)
    {
    }
}
