using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Email;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddStudentGradeCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    [property: JsonIgnore]
    Guid StudentId,
    float Grade,
    int? Percentage,
    GradeType GradeType,
    int? Weight,
    string? Description) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddStudentGradeCommand, Guid>
    {
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public Handler(ISchoolUserRepository schoolUserRepository, ISchoolRepository schoolRepository, IUserService userService, ISchoolUnitOfWork unitOfWork,
            IEmailSender emailSender)
        {
            _schoolUserRepository = schoolUserRepository;
            _schoolRepository = schoolRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task<Result<Guid>> Handle(AddStudentGradeCommand request, CancellationToken cancellationToken)
        {
            var user = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
            NullValidator.ValidateNotNull(@class);

            var student = await _schoolUserRepository.GetStudentByIdAsync(request.StudentId, cancellationToken);
            NullValidator.ValidateNotNull(student);

            if (!@class.StudentIds.Contains(student.Id)
                || @class.Lessons.All(x => x.AssignedTeacher != user.Id))
                return Result<Guid>.BadRequest("Student is not in this class");

            var grade = new Grade(request.Grade, request.Percentage, request.Description, request.GradeType, request.Weight);

            var email = new EmailMessage(student.Email,
                "New Grade Notification",
                $"""
                 <div style="background-color: #f0f0f0; padding: 20px; font-family: Arial, sans-serif; line-height: 1.6;">
                     <div style="max-width: 600px; margin: auto; background: #ffffff; padding: 20px; border-radius: 8px;">
                         <h2 style="color: #333333; text-align: center;">Grade Notification from EduSchedu</h2>
                         <p style="color: #555555;">Hello {student.FullName},</p>
                         <p style="color: #555555;">You have received a new grade in your class. Here are the details:</p>
                         <p style="color: #555555;"><strong>Class:</strong> {@class.Name}</p>
                         <p style="color: #555555;"><strong>Grade Type:</strong> {grade.GradeType}</p>
                         <p style="color: #555555;"><strong>Grade Value:</strong> {grade.GradeValue}</p>
                         {(grade.Weight.HasValue ? $"<p style=\"color: #555555;\"><strong>Weight:</strong> {grade.Weight}</p>" : "")}
                         {(grade.Percentage.HasValue ? $"<p style=\"color: #555555;\"><strong>Percentage:</strong> {grade.Percentage}</p>" : "")}
                         {(string.IsNullOrWhiteSpace(grade.Description) ? "" : $"<p style=\"color: #555555;\"><strong>Description:</strong> {grade.Description}</p>")}
                         <p style="color: #555555;">
                            Keep up the good work, and if you have any questions, please reach out to your teacher or the support team.
                            {user.Email}
                         </p>
                 
                         <p style="color: #555555; margin-top: 20px;">For any assistance, feel free to contact our support team:</p>
                         <div style="text-align: center; margin-top: 10px;">
                             <a href="https://support.eduschedu.com" 
                                style="display: inline-block; background-color: #007bff; color: #ffffff; padding: 10px 20px; border-radius: 5px; text-decoration: none;">
                                Support Team
                             </a>
                         </div>
                     </div>
                 </div>
                 """);

            student.AddGrade(grade);
            await _unitOfWork.CommitAsync(cancellationToken);

            await _emailSender.Send(email);

            return Result<Guid>.Ok(student.Id);
        }
    }
}