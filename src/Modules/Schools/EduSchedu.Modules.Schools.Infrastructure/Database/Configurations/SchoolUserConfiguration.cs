using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class SchoolUserConfiguration : IEntityTypeConfiguration<SchoolUser>
{
    public void Configure(EntityTypeBuilder<SchoolUser> builder)
    {
        builder.ToTable("SchoolUsers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .ValueGeneratedNever();

        builder.Property(x => x.FullName)
            .HasConversion(x => x.Value, x => new Name(x))
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion<string>();

        builder.UseTptMappingStrategy();
    }
}