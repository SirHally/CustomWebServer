using System.Net;
using CustomWebServer.Logic.Types;

namespace CustomWebServer.Logic.Http.ListenerProcessor
{
    /// <summary>
    /// Логика обработки пришедшего HTTP-запроса
    /// </summary>
    public interface  IProcessor
    {
        /// <summary>
        /// Обрабатывает запрос, формируя response
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void Process(ICustomHttpListenerRequest request, ICustomHttpListenerResponse response);
    }
}
