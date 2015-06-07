using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using CustomWebServer.Logic.Types;

namespace CustomWebServer.Logic.Http
{
    /// <summary>
    ///     Логика парсинга POST-запроса для x-www-form-urlencoded
    /// </summary>
    public class PostParamParser
    {
        /// <summary>
        ///     Получаем параметры POST-запроса
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IDictionary<string, string> Parse(ICustomHttpListenerRequest request)
        {
            var postData = new Dictionary<string, string>();
            if (!request.HasEntityBody)
            {
                return postData;
            }

            using (var body = request.InputStream)
            {
                using (var reader = new StreamReader(body, request.ContentEncoding))
                {
                    return Parse(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        ///     Парсинг параметры POST-запроса
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> Parse(string data)
        {
            var postData = new Dictionary<string, string>();

            var requestData = data.Split('&');
            foreach (var keyValue in requestData)
            {
                var kvPair = keyValue.Split('=');
                var key = kvPair[0];
                var value = HttpUtility.UrlDecode(kvPair[1]);
                postData.Add(key, value);
            }

            return postData;
        }
    }
}