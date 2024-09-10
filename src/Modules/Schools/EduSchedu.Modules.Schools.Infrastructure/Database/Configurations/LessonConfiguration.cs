using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new LessonId(x))
            .ValueGeneratedNever();

        builder.Property(x => x.Day)
            .HasConversion<string>();

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();

        builder.OwnsMany(x => x.TeacherIds, ownedBuilder =>
        {
            ownedBuilder.WithOwner().HasForeignKey("LessonId");
            ownedBuilder.ToTable("LessonTeacherIds");
            ownedBuilder.HasKey("Id");

            ownedBuilder.Property(x => x.Value)
                .ValueGeneratedNever()
                .HasColumnName("TeacherId");

            builder.Metadata
                .FindNavigation(nameof(Lesson.TeacherIds))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.OwnsMany(x => x.ClassIds, ownedBuilder =>
        {
            ownedBuilder.WithOwner().HasForeignKey("LessonId");
            ownedBuilder.ToTable("LessonClassIds");
            ownedBuilder.HasKey("Id");

            ownedBuilder.Property(x => x.Value)
                .ValueGeneratedNever()
                .HasColumnName("ClassId");

            builder.Metadata
                .FindNavigation(nameof(Lesson.ClassIds))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}