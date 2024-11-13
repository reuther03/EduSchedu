using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduSchedu.Modules.Users.Infrastructure.Database;

internal class UsersModuleSeeder : IModuleSeeder
{
    private readonly UsersDbContext _dbContext;
    private readonly IPublisher _publisher;

    public UsersModuleSeeder(UsersDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task SeedAsync(IConfiguration configuration, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(x => x.Role == Role.BackOffice, cancellationToken))
        {
            return;
        }

        var userOptions = configuration.GetSection("Backoffice").Get<BackOfficeUserOptions>();
        NullValidator.ValidateNotNull(userOptions);


        var user = User.Create(userOptions.Email, "BackOffice", Password.Create(userOptions.Password), Role.BackOffice);
        await _dbContext.Users.AddAsync(user, cancellationToken);

        await _publisher.Publish(new BackOfficeUserCreatedEvent(user.Id, user.FullName, user.Email, user.Role), cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}