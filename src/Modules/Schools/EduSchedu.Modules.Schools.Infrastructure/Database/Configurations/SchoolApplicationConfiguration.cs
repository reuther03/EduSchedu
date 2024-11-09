using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class SchoolApplicationConfiguration : IEntityTypeConfiguration<SchoolApplication>
{
    public void Configure(EntityTypeBuilder<SchoolApplication> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .ValueGeneratedNever();

        builder.Property(x => x.SubmittedAt)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(500)
            .IsRequired();
    }
}