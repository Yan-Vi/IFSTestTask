using IFSTests.TestObjectModels.API;
using IFSTests.Utils.Logger;

namespace IFSTests.TestObjectModels.App;

public class App(
    ApiClient apiClient,
    AppTestingConfig config,
    ILogger logger
)
{
    public ApiClient ApiClient { get; } = apiClient;
    public AppTestingConfig Config { get; } = config;
    public ILogger Logger = logger;
}