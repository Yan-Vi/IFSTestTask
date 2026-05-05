using System.Text.Json;

namespace IFSTests.Utils.TextFormatting;

public static class JsonFormattingUtils
{
    private static readonly JsonSerializerOptions LogJsonSerializeOptions = new() { WriteIndented = true };

    public static string FormatRequestBodyForLog<T>(T? body)
    {
        if (body is null)
            return "null";
        if (body is string s)
            return FormatJsonForLog(s);
        return JsonSerializer.Serialize(body, LogJsonSerializeOptions);
    }

    public static string FormatJsonForLog(string rawBody)
    {
        if (string.IsNullOrWhiteSpace(rawBody))
            return "(empty)";
        try
        {
            using var doc = JsonDocument.Parse(rawBody);
            return JsonSerializer.Serialize(doc.RootElement, LogJsonSerializeOptions);
        }
        catch (JsonException)
        {
            return rawBody;
        }
    }
}
