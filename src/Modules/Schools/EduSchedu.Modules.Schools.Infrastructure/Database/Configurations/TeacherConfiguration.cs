using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
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

        builder.HasMany(x => x.Lessons)
            .WithOne()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}