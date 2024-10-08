using System.Text.Json;
using RestSharp;

namespace MentalHealthcare.Application.Common;

public class JsonHelper
{
    private readonly Dictionary<string, JsonElement> _jsonDictionary;

    // Constructor: Takes the JSON string and deserializes it into a dictionary
    public JsonHelper(RestResponse restResponse)
    {
        if (restResponse.IsSuccessful && restResponse.Content != null)
        {
            _jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(restResponse.Content);
        }
        else
        {
            throw new ArgumentException("Invalid JSON string provided");
        }
    }

    public JsonHelper(string jsonString)
    {
        _jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString)
                          ?? throw new ArgumentException("Invalid JSON string provided");
    }

    // Method: Get value of a key as string, null if not found or invalid
    public string? GetValue(string key)
    {
        if (_jsonDictionary.TryGetValue(key, out JsonElement valueElement))
        {
            return valueElement.ToString();
        }

        return null;
    }

    // Method: Get value of a key as a strongly-typed object (generic)
    public T? GetValue<T>(string key)
    {
        if (_jsonDictionary.TryGetValue(key, out JsonElement valueElement))
        {
            try
            {
                return JsonSerializer.Deserialize<T>(valueElement.GetRawText());
            }
            catch (JsonException)
            {
                return default; // Return default if deserialization fails
            }
        }

        return default;
    }
}