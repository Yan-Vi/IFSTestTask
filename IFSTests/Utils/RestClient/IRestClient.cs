using IFSTests.Utils.RestClient.ResponseModel;

namespace IFSTests.Utils.RestClient;

public interface IRestClient
{
    Task<Response<TResponse>> GetAsync<TResponse>(Uri requestUri, CancellationToken cancellationToken = default);
    Task<Response<TResponse>> PostAsync<TRequest, TResponse>(Uri requestUri, TRequest requestBody, CancellationToken cancellationToken = default);
    Task<Response<TResponse>> PutAsync<TRequest, TResponse>(Uri requestUri, TRequest requestBody, CancellationToken cancellationToken = default);
    Task<Response<string>> DeleteAsync(Uri requestUri, CancellationToken cancellationToken = default);
}
