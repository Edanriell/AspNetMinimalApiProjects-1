using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;

namespace MinimalWebAPI.Extensions;

public static class ResultExtensions
{
    public static IResult Xml(this IResultExtensions resultExtensions, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new XmlResult(value);
    }
}

public class XmlResult : IResult
{
    private readonly object _value;

    public XmlResult(object value)
    {
        this._value = value;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        using var writer = new StringWriter();
        var serializer = new XmlSerializer(_value.GetType());
        serializer.Serialize(writer, _value);

        var xml = writer.ToString();
        httpContext.Response.ContentType = MediaTypeNames.Application.Xml;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(xml);

        return httpContext.Response.WriteAsync(xml);
    }
}