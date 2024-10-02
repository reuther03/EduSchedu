// using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
//
// namespace EduSchedu.Modules.Schools.Domain.Users;
//
// public class Headmaster : SchoolUser
// {
//     private Headmaster()
//     {
//     }
//
//     private Headmaster(UserId id, Email email, Name fullName, Role role)
//         : base(id, email, fullName, role)
//     {
//     }
//
//     public static Headmaster Create(UserId id, Email email, Name fullName)
//         => new(id, email, fullName, Role.HeadMaster);
// }