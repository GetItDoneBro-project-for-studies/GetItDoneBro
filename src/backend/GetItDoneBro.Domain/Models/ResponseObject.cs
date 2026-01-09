using System.Net.Http.Headers;

namespace GetItDoneBro.Domain.Models;

public class ResponseObject<T>
{
    public int Code { get; set; }
    public T? Value { get; set; }
    public HttpResponseHeaders? Headers { get; set; }
}
