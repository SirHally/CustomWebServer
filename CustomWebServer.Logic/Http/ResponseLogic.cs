using System.IO;
using System.Net;
using System.Text;
using CustomWebServer.Logic.Types;

namespace CustomWebServer.Logic.Http
{
    /// <summary>
    /// Логика формирования тела ответа
    /// </summary>
    public class ResponseLogic
    {
        /// <summary>
        /// Сформировать тело ответа
        /// </summary>
        /// <param name="responseString"></param>
        /// <param name="response"></param>
        public virtual void Execute(string responseString, ICustomHttpListenerResponse response)
        {
            Encoding encoding = Encoding.GetEncoding(1251);
            byte[] buffer = encoding.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.ContentEncoding = encoding;
            
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
