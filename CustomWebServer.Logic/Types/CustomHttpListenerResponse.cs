using System;
using System.IO;
using System.Net;
using System.Text;

namespace CustomWebServer.Logic.Types
{
    /// <summary>
    /// Обертка для HttpListenerResponse
    /// Необходима для тестирования
    /// </summary>
    public class CustomHttpListenerResponse : ICustomHttpListenerResponse
    {
        private readonly HttpListenerResponse _listener;
        public CustomHttpListenerResponse(HttpListenerResponse listener)
        {
            _listener = listener;
        }

        public long ContentLength64
        {
            get { return _listener.ContentLength64; }
            set { _listener.ContentLength64 = value; }
        }

        public Encoding ContentEncoding
        {
            get { return _listener.ContentEncoding; }
            set { _listener.ContentEncoding = value; }
        }
        public string ContentType
        {
            get { return _listener.ContentType; }
            set { _listener.ContentType = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _listener.Headers; }
            set { _listener.Headers = value; }
        }
        public string RedirectLocation
        {
            get { return _listener.RedirectLocation; }
            set { _listener.RedirectLocation = value; }
        }
        public bool KeepAlive
        {
            get { return _listener.KeepAlive; }
            set { _listener.KeepAlive = value; }
        }
        public string StatusDescription
        {
            get { return _listener.StatusDescription; }
            set { _listener.StatusDescription = value; }
        }
        public Version ProtocolVersion
        {
            get { return _listener.ProtocolVersion; }
            set { _listener.ProtocolVersion = value; }
        }
        public int StatusCode
        {
            get { return _listener.StatusCode; }
            set { _listener.StatusCode = value; }
        }

        public CookieCollection Cookies
        {
            get { return _listener.Cookies; }
            set { _listener.Cookies = value; }
        }

        public Stream OutputStream
        {
            get { return _listener.OutputStream; }
        }

    }
}
