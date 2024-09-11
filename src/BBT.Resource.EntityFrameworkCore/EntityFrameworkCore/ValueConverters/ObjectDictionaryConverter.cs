using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBT.Resource.EntityFrameworkCore.ValueConverters;

internal class ObjectDictionaryConverter() : ValueConverter<ObjectDictionary, string>(
    d => SerializeObject(d),
    s => DeserializeObject(s))
{
    public static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions();

    private static string SerializeObject(ObjectDictionary extraProperties)
    {
        var copyDictionary = new Dictionary<string, object?>(extraProperties);
        return JsonSerializer.Serialize(copyDictionary, SerializeOptions);
    }

    public static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions();

    private static ObjectDictionary DeserializeObject(string extraPropertiesAsJson)
    {
        if (extraPropertiesAsJson.IsNullOrEmpty() || extraPropertiesAsJson == "{}")
        {
            return new ObjectDictionary();
        }

        var dictionary = JsonSerializer.Deserialize<ObjectDictionary>(extraPropertiesAsJson, DeserializeOptions) ??
                            new ObjectDictionary();
        
        return dictionary;
    }
}



