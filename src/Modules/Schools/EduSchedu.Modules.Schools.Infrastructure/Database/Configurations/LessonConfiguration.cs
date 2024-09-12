using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Day)
            .HasConversion<string>();

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();

        builder.Property(x => x.AssignedTeacher)
            .HasConversion(x => x!.Value, x => new UserId(x))
            .HasColumnName("AssignedTeacherId");
    }
}