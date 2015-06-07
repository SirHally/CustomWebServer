﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Http.ListenerProcessor;
using CustomWebServer.Logic.Types;
using log4net;

namespace CustomWebServer.Logic.Http
{
    /// <summary>
    /// Логика управления слушателями
    /// </summary>
    public class ListenerLogic
    {
        /// <summary>
        /// Создать слушатель (HttpListener)
        /// </summary>
        /// <param name="prefixes">префиксы, которые будут прослушиваться</param>
        /// <returns></returns>
        private HttpListener Create(IReadOnlyCollection<string> prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Windows XP SP2 или Server 2003 необходимы для использования HttpListener");
            }

            if (prefixes == null || !prefixes.Any())
            {
                throw new ArgumentException("Не указаны префиксы для HttpListener");
            }

            var listener = new HttpListener();

            foreach (var s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            return listener;
        }

        /// <summary>
        /// Стартует новый поток со слушателем.
        /// </summary>
        /// <param name="prefixes"></param>
        /// <param name="callback"></param>
        public void Start(IReadOnlyCollection<string> prefixes, IProcessor callback)
        {
            var listener = Create(prefixes);
            //Каждый слушатель будет обрабатываться независимо, в своем потоке
            var thread = new Thread(Listen);
            thread.Start(new RequestInfo
            {
                Listener = listener,
                Processor = callback
            });
            IoCContainer.Get<ILog>().InfoFormat("Слушатель запущен - {0}.", String.Join(", ", prefixes));
        }

        /// <summary>
        /// Ожидает получения запроса
        /// </summary>
        /// <param name="param"></param>
        private void Listen(object param)
        {
            var data = (RequestInfo) param;
            while (true)
            {
                var result = data.Listener.BeginGetContext(Recieve, data);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Обработка полученного запроса
        /// </summary>
        /// <param name="result"></param>
        private static void Recieve(IAsyncResult result)
        {
            var data = (RequestInfo) result.AsyncState;
            IoCContainer.Get<ILog>().InfoFormat("Получен запрос на слушатель - {0}.", String.Join(", ", data.Listener.Prefixes));
            HttpListenerContext context = data.Listener.EndGetContext(result);

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            try
            {
                data.Processor.Process(new CustomHttpListenerRequest(request), new CustomHttpListenerResponse(response));
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusDescription = "Internal Server Error";
                response.ContentType = "text/plain";
                IoCContainer.Get<ILog>().Error("Произошла ошибка", ex);
                IoCContainer.Get<ResponseLogic>().Execute(ex.Message, new CustomHttpListenerResponse(response));
            }
        }
    }
}