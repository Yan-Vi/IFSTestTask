using IFSTests.Utils.RestClient.Implementations;

namespace IFSTests.Utils.RestClient;

public class RestClientConfig
{
    public string RestClientType { get; init; } = nameof(HttpRestClient);
    public int RequestTimeoutInMs { get; init; } = 10_000;
}
