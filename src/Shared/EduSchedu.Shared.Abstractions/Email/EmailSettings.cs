namespace EduSchedu.Shared.Abstractions.Email;

public sealed class EmailSettings
{
    public const string SectionName = "EmailSettings";

    public int SmtpPort { get; init; } = default!; // 465
    public string SmtpServer { get; init; } = default!; // smtp.outlook.com
    public string FromAddress { get; init; } = default!; // email address - tripmanager@outlook.com
    public string FromName { get; init; } = default!; // Trip Manager
    public string Username { get; init; } = default!; // email address - tripmanager@outlook.com
    public string Password { get; init; } = default!; // password - jakies haslo
}