using System.Net;

namespace IFSTests.Utils.RestClient.ResponseModel;

public sealed class Response<TBody>
{
    public HttpStatusCode StatusCode { get; init; }
    public bool IsSuccessStatusCode { get; init; }
    public TBody? Body { get; init; }
    public string RawBody { get; init; } = string.Empty;
}
