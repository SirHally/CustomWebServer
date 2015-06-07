using System;
using System.IO;
using System.Net;
using System.Text;

namespace CustomWebServer.Logic.Types
{
    /// <summary>
    /// Интерфейс обертки для HttpListenerResponse
    /// Необходим для тестирования
    /// </summary>
    public interface ICustomHttpListenerResponse
    {
        long ContentLength64 { get; set; }
        Encoding ContentEncoding { get; set; }
        string ContentType { get; set; }
        Stream OutputStream { get; }
        WebHeaderCollection Headers { get; set; }
        CookieCollection Cookies { get; set; }
        int StatusCode { get; set; }
        Version ProtocolVersion { get; set; }
        string StatusDescription { get; set; }
        string RedirectLocation { get; set; }
        bool KeepAlive { get; set; }
    }
}