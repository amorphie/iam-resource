using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBT.Resource.EntityFrameworkCore.ValueConverters;

internal class StatusConverter() : ValueConverter<Status, string>(status => status.Code,
    code => Status.FromCode(code));
