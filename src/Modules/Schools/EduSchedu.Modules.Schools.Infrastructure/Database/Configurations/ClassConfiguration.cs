using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ToTable("Classes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => ClassId.From(x))
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new Name(x))
            .IsRequired();

        builder.HasOne(x => x.LanguageProficiency)
            .WithMany()
            .HasForeignKey("LanguageProficiencyId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Lessons)
            .WithOne()
            .HasForeignKey("ClassId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.StudentIds, ownedBuilder =>
        {
            ownedBuilder.WithOwner().HasForeignKey("ClassId");
            ownedBuilder.ToTable("ClassStudentIds");
            ownedBuilder.HasKey("Id");

            ownedBuilder.Property(x => x.Value)
                .ValueGeneratedNever()
                .HasColumnName("StudentId");

            builder.Metadata
                .FindNavigation(nameof(Class.StudentIds))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}