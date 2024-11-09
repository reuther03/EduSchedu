using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class SchoolApplication : Entity<Guid>
{
    public UserId UserId { get; private set; }
    public DateTime SubmittedAt { get; private set; }
    public string Content { get; private set; }

    public SchoolApplication()
    {
    }

    private SchoolApplication(Guid id, UserId userId, string content) : base(id)
    {
        UserId = userId;
        SubmittedAt = DateTime.UtcNow;
        Content = content;
    }

    public static SchoolApplication Create(UserId userId, string content)
        => new(Guid.NewGuid(), userId, content);
}