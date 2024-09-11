using BBT.Resource.Policies;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBT.Resource.EntityFrameworkCore.ValueConverters;

internal class ConflictResolutionConverter() : ValueConverter<ConflictResolution, string>(effect => effect.Code,
    code => ConflictResolution.FromCode(code));
