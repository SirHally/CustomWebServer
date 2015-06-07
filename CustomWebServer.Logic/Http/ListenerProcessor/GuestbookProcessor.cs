using System;
using System.Linq;
using System.Net;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Types;
using log4net;
using Newtonsoft.Json;

namespace CustomWebServer.Logic.Http.ListenerProcessor
{
    /// <summary>
    /// Обработка запроса к гостевой книге
    /// </summary>
    public class GuestbookProcessor : IProcessor
    {
        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="response">Ответ</param>
        public void Process(ICustomHttpListenerRequest request, ICustomHttpListenerResponse response)
        {
            string responseString = String.Empty;
            var logic = IoCContainer.Get<GuestbookLogic>();
            if (request.HttpMethod == "POST")
            {
                //Создание новой записи
                const string userKey = "user";
                const string textKey = "message";

                var postData = IoCContainer.Get<PostParamParser>().Parse(request);

                string userName;
                if (postData.ContainsKey(userKey))
                {
                    userName = postData[userKey];
                }
                else
                {
                    throw new ArgumentException("В POST запросе должен быть задан параметр " + userKey);
                }

                string text;
                if (postData.ContainsKey(textKey))
                {
                    text = postData[textKey];
                }
                else
                {
                    throw new ArgumentException("В POST запросе должен быть задан параметр " + textKey);
                }
                logic.CreateMessage(userName, text);

                IoCContainer.Get<ILog>().InfoFormat("Создана новая запись. Пользователь - {0}, сообщение - {1}", userName, text);
            } else if (request.HttpMethod == "GET")
            {
                //Отдавать все записи
                var messages = logic.GetAll();

                IoCContainer.Get<ILog>().InfoFormat("Возврат всех записей гостевой книги. Количество - {0}.", messages.Count());
                responseString = JsonConvert.SerializeObject(messages);
                response.ContentType = "application/json";
            }
            else
            {
                throw new ArgumentException("Для Guestbook поддерживаются только методы GET и POST");
            }
            IoCContainer.Get<ResponseLogic>().Execute(responseString,  response);
        }
    }
}