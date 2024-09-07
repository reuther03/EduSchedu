using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddLanguageProficiencyCommand(Guid SchoolId) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddLanguageProficiencyCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ILanguageProficiencyRepository _languageProficiencyRepository;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(ISchoolRepository schoolRepository, ILanguageProficiencyRepository languageProficiencyRepository, ISchoolUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _languageProficiencyRepository = languageProficiencyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddLanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}