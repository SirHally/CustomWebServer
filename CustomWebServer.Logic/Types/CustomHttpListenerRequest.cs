using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace CustomWebServer.Logic.Types
{
    /// <summary>
    /// Обертка для HttpListenerRequest
    /// Необходима для тестирования
    /// </summary>
    public class CustomHttpListenerRequest : ICustomHttpListenerRequest
    {
        private readonly HttpListenerRequest _listener;
        public CustomHttpListenerRequest(HttpListenerRequest request)
        {
            _listener = request;
        }

        public string HttpMethod
        {
            get { return _listener.HttpMethod; }
        }

        public bool HasEntityBody
        {
            get { return _listener.HasEntityBody; }
        }

        public NameValueCollection QueryString
        {
            get { return _listener.QueryString; }
        }

        public Stream InputStream
        {
            get { return _listener.InputStream; }
        }

        public Encoding ContentEncoding
        {
            get { return _listener.ContentEncoding; }
        }
    }
}
