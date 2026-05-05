using System.Text.Json;

namespace IFSTests.Utils.Configs;

public static class ConfigLoader
{
    private const string _defaultConfigName = "default";
    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static TConfig Load<TConfig>(string? configName = null)
    {
        configName = ResolveConfigName(configName);
        var fileName = $"{configName}.config.json";
        var configPath = Path.Combine(AppContext.BaseDirectory, "TestConfigurations", fileName);
        using var configStream = File.OpenRead(configPath);
        var configuration = JsonSerializer.Deserialize<TConfig>(configStream, _serializerOptions);
        return configuration ?? throw new InvalidOperationException($"Configuration '{fileName}' is empty.");
    }

    private static string ResolveConfigName(string? explicitConfigName = null)
    {
        if (!string.IsNullOrWhiteSpace(explicitConfigName))
        {
            return explicitConfigName;
        }

        var configNameFromNUnit = TestContext.Parameters["configName"];
        if (!string.IsNullOrWhiteSpace(configNameFromNUnit))
        {
            return configNameFromNUnit;
        }

        var configNameFromEnvironment = Environment.GetEnvironmentVariable("IFS_TEST_CONFIG");
        if (!string.IsNullOrWhiteSpace(configNameFromEnvironment))
        {
            return configNameFromEnvironment;
        }

        return _defaultConfigName;
    }
}
