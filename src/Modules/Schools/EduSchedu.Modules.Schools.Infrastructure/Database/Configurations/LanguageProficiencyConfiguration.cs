using EduSchedu.Modules.Schools.Domain;
using EduSchedu.Modules.Schools.Domain.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class LanguageProficiencyConfiguration : IEntityTypeConfiguration<LanguageProficiency>
{
    public void Configure(EntityTypeBuilder<LanguageProficiency> builder)
    {
        builder.ToTable("LanguageProficiencies");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Language)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Lvl)
            .HasConversion<string>()
            .IsRequired();
    }
}