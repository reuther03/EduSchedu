using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
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
        });
    }
}