using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBT.Resource.EntityFrameworkCore.ValueConverters;

internal class StringArrayConverter() : ValueConverter<string[]?, string>(v => (v == null ? null : string.Join(',', v)) ?? string.Empty,
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
