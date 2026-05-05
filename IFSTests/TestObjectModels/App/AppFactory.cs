using IFSTests.TestObjectModels.API;
using IFSTests.TestObjectModels.API.Posts;
using IFSTests.Utils.Configs;
using IFSTests.Utils.Logger;
using IFSTests.Utils.RestClient;

namespace IFSTests.TestObjectModels.App;

public static class AppFactory
{
    public static App Create(string? configName = null)
    {
        var testConfig = ConfigLoader.Load<AppTestingConfig>(configName);
        var logger = LoggerFactory.Create(testConfig.LoggerType);
        var restClient = RestClientFactory.Create(testConfig.ApiClientConfig.RestClientConfig, logger);
        var postsController = new PostsController(restClient, testConfig, testConfig.ApiClientConfig.PostsControllerConfig);
        var apiClient = new ApiClient(postsController);
        return new App(apiClient, testConfig, logger);
    }
}
