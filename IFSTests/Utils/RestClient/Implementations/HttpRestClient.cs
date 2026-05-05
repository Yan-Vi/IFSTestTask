using System.Diagnostics;
using System.Net.Http.Json;
using IFSTests.Utils.Logger;
using IFSTests.Utils.RestClient.ResponseModel;
using IFSTests.Utils.TextFormatting;

namespace IFSTests.Utils.RestClient.Implementations;

public sealed class HttpRestClient(HttpClient httpClient, ILogger logger) : IRestClient
{
    public Task<Response<TResponse>> GetAsync<TResponse>
    (
        Uri requestUri,
        CancellationToken cancellationToken = default
    )
    {
        return logger.StepAsync($"GET {requestUri}", async () =>
        {
            logger.Info("Request GET {0} - Body: (none)", requestUri);
            var startedAt = Stopwatch.GetTimestamp();
            using var response = await httpClient.GetAsync(requestUri, cancellationToken);
            var elapsedMs = GetElapsedMilliseconds(startedAt);
            var restResponse = await response.ToRestResponseAsync<TResponse>(cancellationToken);
            logger.Info(
                "Response GET {0} - Status: {1} ({2} ms) - Body:\n{3}",
                requestUri,
                (int)restResponse.StatusCode,
                elapsedMs,
                JsonFormattingUtils.FormatJsonForLog(restResponse.RawBody)
            );
            return restResponse;
        });
    }

    public Task<Response<TResponse>> PostAsync<TRequest, TResponse>
    (
        Uri requestUri,
        TRequest requestBody,
        CancellationToken cancellationToken = default
    )
    {
        return logger.StepAsync($"POST {requestUri}", async () =>
        {
            logger.Info(
                "Request POST {0} - Body:\n{1}",
                requestUri,
                JsonFormattingUtils.FormatRequestBodyForLog(requestBody)
            );
            var startedAt = Stopwatch.GetTimestamp();
            using var response = await httpClient.PostAsJsonAsync(requestUri, requestBody, cancellationToken);
            var elapsedMs = GetElapsedMilliseconds(startedAt);
            var restResponse = await response.ToRestResponseAsync<TResponse>(cancellationToken);
            logger.Info(
                "Response POST {0} - Status: {1} ({2} ms) - Body:\n{3}",
                requestUri,
                (int)restResponse.StatusCode,
                elapsedMs,
                JsonFormattingUtils.FormatJsonForLog(restResponse.RawBody)
            );
            return restResponse;
        });
    }

    public Task<Response<TResponse>> PutAsync<TRequest, TResponse>
    (
        Uri requestUri,
        TRequest requestBody,
        CancellationToken cancellationToken = default
    )
    {
        return logger.StepAsync($"PUT {requestUri}", async () =>
        {
            logger.Info(
                "Request PUT {0} - Body:\n{1}",
                requestUri,
                JsonFormattingUtils.FormatRequestBodyForLog(requestBody)
            );
            var startedAt = Stopwatch.GetTimestamp();
            using var response = await httpClient.PutAsJsonAsync(requestUri, requestBody, cancellationToken);
            var elapsedMs = GetElapsedMilliseconds(startedAt);
            var restResponse = await response.ToRestResponseAsync<TResponse>(cancellationToken);
            logger.Info(
                "Response PUT {0} - Status: {1} ({2} ms) - Body:\n{3}",
                requestUri,
                (int)restResponse.StatusCode,
                elapsedMs,
                JsonFormattingUtils.FormatJsonForLog(restResponse.RawBody)
            );
            return restResponse;
        });
    }

    public Task<Response<string>> DeleteAsync
    (
        Uri requestUri,
        CancellationToken cancellationToken = default
    )
    {
        return logger.StepAsync($"DELETE {requestUri}", async () =>
        {
            logger.Info("Request DELETE {0} - Body: (none)", requestUri);
            var startedAt = Stopwatch.GetTimestamp();
            using var response = await httpClient.DeleteAsync(requestUri, cancellationToken);
            var elapsedMs = GetElapsedMilliseconds(startedAt);
            var restResponse = await response.ToRestResponseAsync<string>(cancellationToken);
            logger.Info(
                "Response DELETE {0} - Status: {1} ({2} ms) - Body:\n{3}",
                requestUri,
                (int)restResponse.StatusCode,
                elapsedMs,
                JsonFormattingUtils.FormatJsonForLog(restResponse.RawBody)
            );
            return restResponse;
        });
    }

    private static long GetElapsedMilliseconds(long startedAtTimestamp)
    {
        return (long)TimeSpan
            .FromSeconds((Stopwatch.GetTimestamp() - startedAtTimestamp) / (double)Stopwatch.Frequency)
            .TotalMilliseconds;
    }
}
