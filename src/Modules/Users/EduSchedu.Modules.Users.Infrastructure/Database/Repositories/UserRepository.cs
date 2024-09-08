using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Users.Infrastructure.Database.Repositories;

internal class UserRepository : Repository<User, UsersDbContext>, IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
}