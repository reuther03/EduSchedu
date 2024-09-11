using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => ScheduleId.From(x))
            .ValueGeneratedNever();

        builder.Property(x => x.TeacherId)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .ValueGeneratedNever();

        builder.HasOne(x => x.Teacher)
            .WithOne(x => x.Schedule)
            .HasForeignKey<Schedule>(x => x.TeacherId)
            .IsRequired();
    }
}