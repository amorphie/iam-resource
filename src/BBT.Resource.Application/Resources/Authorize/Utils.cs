using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Elastic.Apm.Api;
using Newtonsoft.Json;

namespace BBT.Resource.Resources.Authorize;

public static class Utils
{
    public static int ToInt(this object value)
    {
        try
        {
            return Convert.ToInt32(value);
        }
        catch
        {
            return 0;
        }
    }

    public static double ToDouble(this object value)
    {
        try
        {
            return Convert.ToDouble(value);
        }
        catch
        {
            return 0d;
        }
    }

    public static float ToFloat(this object value)
    {
        try
        {
            return (float)value;
        }
        catch
        {
            return 0f;
        }
    }

    public static bool ToBool(this object value)
    {
        try
        {
            return Convert.ToBoolean(value);
        }
        catch
        {
            return false;
        }
    }

    public static DateTime ToDateTime(this object value)
    {
        try
        {
            return Convert.ToDateTime(value);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    public static string[] ToArray(this object value)
    {
        try
        {
            return JsonConvert.DeserializeObject<string[]>(value.ToString());
        }
        catch
        {
            return default;
        }
    }

    public static bool CheckContains(string check, string valList)
    {
        if (string.IsNullOrEmpty(check) || string.IsNullOrEmpty(valList))
            return false;

        var list = valList.Split(',').ToList();
        return list.Contains(check);
    }

    public static CallApiResponse CallApiGet(string url)
    {
        return CallApi(url, null, ResourceType.GET, null);
    }

    public static CallApiResponse CallApiGet(string url, dynamic header)
    {
        return CallApi(url, null, ResourceType.GET, header);
    }

    public static CallApiResponse CallApiPost(string url, dynamic body)
    {
        return CallApi(url, body, ResourceType.POST, null);
    }

    public static CallApiResponse CallApiPost(string url, dynamic body, dynamic header)
    {
        return CallApi(url, body, ResourceType.POST, header);
    }

    private static CallApiResponse CallApi(
        string url,
        dynamic body,
        ResourceType httpMethodType,
        dynamic? header = null)
    {
        var apiClientService = new HttpClientService();
        var response = new HttpResponseMessage();
        dynamic data = null;

        var transaction = Elastic.Apm.Agent.Tracer.CurrentTransaction ??
                          Elastic.Apm.Agent.Tracer.StartTransaction("CallApi", ApiConstants.TypeUnknown);
        var span = transaction.StartSpan($"CallApi-{url}", ApiConstants.TypeUnknown);

        Task.Run(async () =>
        {
            try
            {
                if (body == null)
                    body = "";

                response = await apiClientService.SendRequestAsync(
                    url,
                    body,
                    httpMethodType.ToString(),
                    header,
                    span);

                span.SetLabel("CallApi.Response.StatusCode", response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a JSON string
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    span.SetLabel("CallApi.Response.Body", jsonContent);
                    using (JsonDocument jsonDocument = JsonDocument.Parse(jsonContent))
                    {
                        // Convert the JsonDocument to a dynamic object
                        data = DeserializeJsonDocument(jsonDocument);
                    }
                }
            }
            catch (Exception e)
            {
                span.CaptureException(e);
            }
            finally
            {
                span.End();
            }
        }).Wait();

        var result = new CallApiResponse() { IsSuccessStatusCode = response.IsSuccessStatusCode, Data = data };

        return result;
    }

    static dynamic DeserializeJsonDocument(JsonDocument jsonDocument)
    {
        // Create an empty dynamic object
        dynamic dynamicObject = new System.Dynamic.ExpandoObject();

        // Iterate over each property in the JsonDocument
        foreach (JsonProperty property in jsonDocument.RootElement.EnumerateObject())
        {
            // Add the property to the dynamic object
            ((IDictionary<string, object?>)dynamicObject)[property.Name] = GetValue(property.Value);
        }

        return dynamicObject;
    }

    static object? GetValue(JsonElement jsonElement)
    {
        // Handle different value types
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Number:
                return jsonElement.GetDouble(); // Or GetInt32(), GetInt64(), etc. depending on your needs
            case JsonValueKind.String:
                return jsonElement.GetString();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
                return null;
            default:
                return jsonElement.ToString();
        }
    }
}

public class CallApiResponse
{
    public bool IsSuccessStatusCode;
    public dynamic Data;
}
