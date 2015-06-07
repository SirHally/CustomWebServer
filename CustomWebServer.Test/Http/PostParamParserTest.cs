using System.Collections.Generic;
using System.Net;
using CustomWebServer.Logic.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomWebServer.Test.Http
{
    /// <summary>
    /// Тесты для логики PostParamParser
    /// </summary>
    [TestClass]
    public class PostParamParserTest
    {
        /// <summary>
        /// Тестируем парсинг http post запроса
        /// </summary>
        [TestMethod]
        public void PostParamParser_Parse()
        {
            var logic = new PostParamParser();
            const string post = "user=sirhally&message=Hello";
            IDictionary<string, string> data = logic.Parse(post);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("sirhally", data["user"]);
            Assert.AreEqual("Hello", data["message"]);
        }
    }
}
