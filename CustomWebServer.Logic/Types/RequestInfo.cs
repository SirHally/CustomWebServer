using System.Net;
using CustomWebServer.Logic.Http.ListenerProcessor;

namespace CustomWebServer.Logic.Types
{
    /// <summary>
    /// Информация о HttpListener, передающаяся в поток его обработки
    /// </summary>
    public struct RequestInfo
    {
        /// <summary>
        /// Прослушиватель
        /// </summary>
        public HttpListener Listener { get; set; }

        /// <summary>
        /// Логика обработки
        /// </summary>
        public IProcessor Processor { get; set; }
    }
}