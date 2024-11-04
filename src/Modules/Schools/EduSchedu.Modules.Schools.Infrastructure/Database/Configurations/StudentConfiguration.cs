using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.OwnsMany(x => x.Grades, grade =>
        {
            grade.Property(x => x.GradeValue)
                .HasColumnName("Grade")
                .IsRequired();

            grade.Property(x => x.Percentage)
                .IsRequired(false);

            grade.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            grade.Property(x => x.GradeType)
                .HasConversion<string>()
                .IsRequired();

            grade.Property(x => x.Weight)
                .IsRequired(false);

            grade.Property(x => x.AssignedBy)
                .HasConversion(x => x.Value, x => new UserId(x))
                .ValueGeneratedNever()
                .IsRequired();

            grade.Property(x => x.AssignedAt)
                .IsRequired();
        });

        builder.Property(x => x.AverageGrade)
            .HasPrecision(5, 3)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}