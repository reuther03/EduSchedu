using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Shared.Abstractions.Email;

public interface IEmailSender
{
    public Task Send(EmailMessage request);
}