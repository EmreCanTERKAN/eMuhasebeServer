namespace eMuhasebeServer.Domain.Dtos;
public sealed record InvoiceDetailDto
{
    public DateOnly Date { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}
