using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Types;
using log4net;

namespace CustomWebServer.Logic.Http.ListenerProcessor
{
    /// <summary>
    /// Обработчик запроса к Proxy
    /// </summary>
    public class ProxyProcessor : IProcessor
    {
        /// <summary>
        /// Обрабатываем запрос к Proxy
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void Process(ICustomHttpListenerRequest request, ICustomHttpListenerResponse response)
        {
            string url = GetUrl(request);
            MakeResponse(url, response);
        }

        /// <summary>
        /// Получаем URL из запроса
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetUrl(ICustomHttpListenerRequest request)
        {
            const string urlKey = "url";
            if (request.HttpMethod == "POST")
            {
                var postData = IoCContainer.Get<PostParamParser>().Parse(request);
                if (postData.ContainsKey(urlKey))
                {
                    return postData[urlKey];
                }
                else
                {
                    throw new ArgumentException("Должен быть задан параметр " + urlKey);
                }
            }
            else if (request.HttpMethod == "GET")
            {
                if (request.QueryString.AllKeys.Contains(urlKey))
                {
                    IoCContainer.Get<ILog>().InfoFormat("Запрос Proxy для адреса - {0}", request.QueryString[urlKey]);
                    return request.QueryString[urlKey];
                }
                else
                {
                    throw new ArgumentException("Должен быть задан параметр " + urlKey);
                }
            }
            else
            {
                throw new ArgumentException("Для Proxy поддерживаются только методы GET и POST");
            }
        }

        /// <summary>
        /// Загрузка страницы по заданному URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpWebResponse LoadPage(string url)
        {

            HttpWebRequest g = WebRequest.CreateHttp(url);
            HttpWebResponse netResponse = (HttpWebResponse)g.GetResponse();
            return netResponse;
        }

        /// <summary>
        /// Формируем ответ
        /// </summary>
        /// <param name="url"></param>
        /// <param name="response"></param>
        private void MakeResponse(string url, ICustomHttpListenerResponse response)
        {
            var netResponse = LoadPage(url);
            response.ContentType = netResponse.ContentType;
            response.ContentLength64 = netResponse.ContentLength;
            foreach (string header in netResponse.Headers)
            {
                if (header != "Location" && header != "Keep-Alive" && header != "Content-Type" && header != "Content-Length")
                {
                     response.Headers.Add(header, netResponse.Headers.Get(header));
                }
            }
            CopyStream(netResponse.GetResponseStream(),response.OutputStream);
            response.OutputStream.Close();
            if (!String.IsNullOrEmpty(netResponse.ContentEncoding))
            {
                response.ContentEncoding = Encoding.GetEncoding(netResponse.ContentEncoding);
            }
            
            response.Cookies = netResponse.Cookies;
            response.StatusCode = (int)netResponse.StatusCode;
            response.ProtocolVersion = netResponse.ProtocolVersion;
            response.StatusDescription = netResponse.StatusDescription;
            response.RedirectLocation = netResponse.Headers.Get("Location");
            response.KeepAlive = netResponse.Headers.Get("Keep-Alive") != null;
        }

        /// <summary>
        /// Копирование содержимого потоков
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private int CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int bytesWritten = 0;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    break;
                output.Write(buffer, 0, read);
                bytesWritten += read;
            }
            return bytesWritten;
        }
    }
}