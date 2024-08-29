using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Users.Infrastructure.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
        _users = context.Users;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _users.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await _users.AddAsync(user, cancellationToken);
}