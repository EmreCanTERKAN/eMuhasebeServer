using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CustomerDetails;
public sealed record GetAllCustomerDetailQuery(
    Guid CustomerId) : IRequest<Result<Customer>>;


internal sealed class GetAllCustomerDetailQueryHandler(
    ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomerDetailQuery, Result<Customer>>
{
    public async Task<Result<Customer>> Handle(GetAllCustomerDetailQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository
            .Where(p => p.Id == request.CustomerId)
            .Include(p => p.CustomerDetails)
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        {
            return Result<Customer>.Failure("Cari bulunamadı");
        }
        return customer;
    }
}
