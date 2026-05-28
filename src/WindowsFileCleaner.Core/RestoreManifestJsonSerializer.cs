using System.Text.Json;
using System.Text.Json.Serialization;

namespace WindowsFileCleaner.Core;

public static class RestoreManifestJsonSerializer
{
    public static string Serialize(RestoreManifest manifest)
    {
        return JsonSerializer.Serialize(manifest, CreateOptions());
    }

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
