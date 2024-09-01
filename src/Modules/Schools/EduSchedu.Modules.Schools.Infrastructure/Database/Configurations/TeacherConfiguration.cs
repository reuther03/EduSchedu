using EduSchedu.Modules.Schools.Domain;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

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
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsMany(x => x.LanguageProficiencyIds, ownedBuilder =>
        {
            ownedBuilder.WithOwner().HasForeignKey("TeacherId");
            ownedBuilder.ToTable("TeacherLanguageProficiencyIds");
            ownedBuilder.HasKey("Id");

            ownedBuilder.Property(x => x.Value)
                .ValueGeneratedNever()
                .HasColumnName("LanguageProficiencyId");

            builder.Metadata
                .FindNavigation(nameof(Teacher.LanguageProficiencyIds))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}