using EduSchedu.Modules.Schedules.Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schedules.Application.Abstractions.Database;

public interface ISchedulesDbContext
{
    DbSet<Schedule> Schedules { get; }
}