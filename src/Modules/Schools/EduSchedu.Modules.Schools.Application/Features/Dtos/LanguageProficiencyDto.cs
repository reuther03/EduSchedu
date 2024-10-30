using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class LanguageProficiencyDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Language Language { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]

    public Lvl Lvl { get; init; }

    public static LanguageProficiencyDto AsDto(LanguageProficiency languageProficiency)
    {
        return new LanguageProficiencyDto
        {
            Language = languageProficiency.Language,
            Lvl = languageProficiency.Lvl
        };
    }
}