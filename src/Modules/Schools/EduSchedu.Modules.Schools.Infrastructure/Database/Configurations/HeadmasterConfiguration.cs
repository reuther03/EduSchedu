using EduSchedu.Modules.Schools.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class HeadmasterConfiguration : IEntityTypeConfiguration<Headmaster>
{
    public void Configure(EntityTypeBuilder<Headmaster> builder)
    {
    }
}