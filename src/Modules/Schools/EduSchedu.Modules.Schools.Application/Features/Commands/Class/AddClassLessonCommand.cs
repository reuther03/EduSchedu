using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public class AddClassLessonCommand : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddClassLessonCommand, Guid>
    {
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(
            ISchoolUserRepository schoolUserRepository,
            IUserService userService,
            ISchoolRepository schoolRepository,
            ILessonRepository lessonRepository,
            ISchoolUnitOfWork unitOfWork)
        {
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _schoolRepository = schoolRepository;
            _lessonRepository = lessonRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Guid>> Handle(AddClassLessonCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}