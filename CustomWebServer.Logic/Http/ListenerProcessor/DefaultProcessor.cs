using System.Net;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Types;
using log4net;

namespace CustomWebServer.Logic.Http.ListenerProcessor
{
    /// <summary>
    /// Формируем ответ на запросы по умолчанию
    /// </summary>
    public class DefaultProcessor : IProcessor
    {
        public const string ResponseString = "Hello world!";
        /// <summary>
        /// Обработка запроса
        /// </summary>
        /// <param name="request">Входные параметры запроса</param>
        /// <param name="response">Выходные параметры запроса</param>
        public void Process(ICustomHttpListenerRequest request, ICustomHttpListenerResponse response)
        {
            IoCContainer.Get<ResponseLogic>().Execute(ResponseString,  response);
            IoCContainer.Get<ILog>().InfoFormat("Возврат стандартного сообщения - {0}", ResponseString);
        }
    }
}
