using BBT.Resource.Policies;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBT.Resource.EntityFrameworkCore.ValueConverters;

internal class EffectConverter() : ValueConverter<Effect, string>(effect => effect.Code,
    code => Effect.FromCode(code));
