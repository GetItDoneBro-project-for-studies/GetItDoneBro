namespace GetItDoneBro.Domain.Models;

public class Request(string host, Uri uri)
{
    public Request(string host, Uri uri, object data)
        : this(host: host, uri: uri)
    {
        Data = data;
    }

    public string Host { get; } = host;
    public Uri Uri { get; } = uri;
    public object? Data { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);
}
