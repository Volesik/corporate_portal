using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace CorporatePortal.BL.Helpers;

public static class AppJsonSerializerOptions
{
    private static readonly JsonSerializerOptions _default = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new ObjectToInferredTypesConverter()
        }
    };
    
    public static JsonSerializerOptions Default => _default;

    public static void Apply(JsonOptions options)
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }
}