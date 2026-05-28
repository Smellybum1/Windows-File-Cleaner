using System.Text.Json;
using System.Text.Json.Serialization;

namespace WindowsFileCleaner.Core;

public static class RestoreManifestDraftJsonSerializer
{
    public static string Serialize(RestoreManifestDraft draft)
    {
        return JsonSerializer.Serialize(draft, CreateOptions());
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
