using IFSTests.TestObjectModels.API;
using IFSTests.Utils.Logger.Implementations;

namespace IFSTests.TestObjectModels.App;

public class AppTestingConfig
{
    public string BaseUrl { get; init; } = string.Empty;
    public string LoggerType { get; init; } = nameof(NullLogger);
    public ApiClientConfig ApiClientConfig { get; init; } = new();
}
