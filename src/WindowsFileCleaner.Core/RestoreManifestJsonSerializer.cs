using System.Text.Json;
using System.Text.Json.Serialization;

namespace WindowsFileCleaner.Core;

public static class RestoreManifestJsonSerializer
{
    public static string Serialize(RestoreManifest manifest)
    {
        return JsonSerializer.Serialize(manifest, CreateOptions());
    }

    public static RestoreManifest Deserialize(string json)
    {
        var manifest = JsonSerializer.Deserialize<RestoreManifest>(json, CreateOptions());
        return manifest ?? throw new InvalidOperationException("Restore Manifest JSON did not contain a manifest.");
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
