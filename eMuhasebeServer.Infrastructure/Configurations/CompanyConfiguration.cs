using eMuhasebeServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eMuhasebeServer.Infrastructure.Configurations;
internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(x => x.TaxNumber).HasColumnType("varchar(11)");
        builder.HasQueryFilter(x => x.IsDeleted == false);

        builder.OwnsOne(x => x.Database);
    }
}
