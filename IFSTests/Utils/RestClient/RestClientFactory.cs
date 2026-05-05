using IFSTests.Utils.Logger;
using IFSTests.Utils.Logger.Implementations;
using IFSTests.Utils.RestClient.Implementations;

namespace IFSTests.Utils.RestClient;

public static class RestClientFactory
{
    public static IRestClient Create(RestClientConfig config, ILogger logger)
    {
        HttpClient httpClient;
        switch (config.RestClientType)
        {
            case nameof(HttpRestClient):
                httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(config.RequestTimeoutInMs);
                return new HttpRestClient(httpClient, logger);
            default:
                new ConsoleLogger().Error(
                    "Unsupported rest client type '{0}'. Falling back to '{1}'.",
                    config.RestClientType,
                    nameof(HttpRestClient));
                httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(config.RequestTimeoutInMs);
                return new HttpRestClient(httpClient, logger);
        }
    }
}
