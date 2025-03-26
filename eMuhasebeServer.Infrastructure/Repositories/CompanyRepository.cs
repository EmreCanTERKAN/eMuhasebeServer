using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;

namespace eMuhasebeServer.Infrastructure.Repositories;
internal sealed class CompanyRepository : Repository<Company, ApplicationDbContext>, IRepository<Company>
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
