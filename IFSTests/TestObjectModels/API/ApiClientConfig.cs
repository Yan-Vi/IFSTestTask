using IFSTests.TestObjectModels.API.Posts;
using IFSTests.Utils.RestClient;

namespace IFSTests.TestObjectModels.API;

public class ApiClientConfig
{
    public RestClientConfig RestClientConfig { get; init; } = new();
    public PostsControllerConfig PostsControllerConfig { get; init; } = new();
}
