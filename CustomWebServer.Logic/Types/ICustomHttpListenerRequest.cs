using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace CustomWebServer.Logic.Types
{
    /// <summary>
    /// Интерфейс обертки для HttpListenerRequest
    /// Необходим для тестирования
    /// </summary>
    public interface ICustomHttpListenerRequest
    {
        string HttpMethod { get; }
        bool HasEntityBody { get; }
        NameValueCollection QueryString { get; }
        Stream InputStream { get; }
        Encoding ContentEncoding { get; }
    }
}