using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.ToTable("Schools");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => SchoolId.From(x))
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(300)
            .HasConversion(x => x.Value, x => new Name(x))
            .IsRequired();

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.City)
                .HasColumnName("City")
                .IsRequired();

            address.Property(x => x.Street)
                .HasColumnName("Street")
                .IsRequired();

            address.Property(x => x.ZipCode)
                .HasColumnName("ZipCode")
                .IsRequired();

            address.Property(x => x.MapCoordinates)
                .HasColumnName("MapCoordinates")
                .IsRequired(false);
        });

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired();

        builder.Property(x => x.PrincipalId)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .IsRequired();

        builder.OwnsMany(x => x.ClassIds, ownedBuilder =>
        {
            ownedBuilder.WithOwner().HasForeignKey("SchoolId");
            ownedBuilder.ToTable("SchoolClassIds");
            ownedBuilder.HasKey("Id");

            ownedBuilder.Property(x => x.Value)
                .ValueGeneratedNever()
                .HasColumnName("ClassId");

            builder.Metadata
                .FindNavigation(nameof(School.ClassIds))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.OwnsMany(x => x.TeacherIds, ownedBuilder =>
        {
            ownedBuilder.WithOwner().HasForeignKey("SchoolId");
            ownedBuilder.ToTable("SchoolTeacherIds");
            ownedBuilder.HasKey("Id");

            ownedBuilder.Property(x => x.Value)
                .ValueGeneratedNever()
                .HasColumnName("TeacherId");

            builder.Metadata
                .FindNavigation(nameof(School.TeacherIds))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}