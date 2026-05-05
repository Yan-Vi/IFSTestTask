using System.Text.Json;

namespace IFSTests.Utils.RestClient.ResponseModel;

public static class ResponseExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task<Response<TResponse>> ToRestResponseAsync<TResponse>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var rawBody = await response.Content.ReadAsStringAsync(cancellationToken);

        TResponse? body;
        if (string.IsNullOrWhiteSpace(rawBody))
        {
            body = CreateEmptyArrayIfNeeded<TResponse>();
        }
        else if (typeof(TResponse) == typeof(string))
        {
            body = (TResponse)(object)rawBody;
        }
        else
        {
            body = JsonSerializer.Deserialize<TResponse>(rawBody, _serializerOptions);
            body ??= CreateEmptyArrayIfNeeded<TResponse>();
        }

        return new Response<TResponse>
        {
            StatusCode = response.StatusCode,
            IsSuccessStatusCode = response.IsSuccessStatusCode,
            Body = body,
            RawBody = rawBody
        };
    }

    private static TResponse? CreateEmptyArrayIfNeeded<TResponse>()
    {
        if (!typeof(TResponse).IsArray)
        {
            return default;
        }

        var elementType = typeof(TResponse).GetElementType() ?? typeof(object);
        return (TResponse)(object)Array.CreateInstance(elementType, 0);
    }
}
