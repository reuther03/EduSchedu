using System.Diagnostics.CodeAnalysis;

namespace EduSchedu.Shared.Abstractions.Kernel.CommandValidators;

public static class NullValidator
{
    public static void ValidateNotNull<T>([NotNull] T value)
    {
        if (value is null)
            throw new ArgumentNullException($"{typeof(T).Name} is null");
    }
}