using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSchedu.Modules.Schedules.Infrastructure.Database.Configurations;

public class SchedulesConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => ScheduleId.From(x))
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => UserId.From(x))
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasMany(x => x.ScheduleItems)
            .WithOne()
            .HasForeignKey("ScheduleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId);
    }
}