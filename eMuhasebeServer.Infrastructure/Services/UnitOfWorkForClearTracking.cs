// Infrastructure Katmanı
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Infrastructure.Context;

internal class UnitOfWorkForClearTracking : IUnitOfWorkForClearTracking
{
    private readonly CompanyDbContext _dbContext;

    public UnitOfWorkForClearTracking(CompanyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ClearTracking()
    {
        _dbContext.ChangeTracker.Clear(); // Burada direkt kullanıyoruz çünkü aynı assembly'deyiz
    }
}
