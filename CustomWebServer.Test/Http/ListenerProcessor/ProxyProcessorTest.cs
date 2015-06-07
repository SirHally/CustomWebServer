using System.Collections.Generic;
using System.Collections.Specialized;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Http;
using CustomWebServer.Logic.Http.ListenerProcessor;
using CustomWebServer.Logic.Types;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CustomWebServer.Test.Http.ListenerProcessor
{
    [TestClass]
    public class ProxyProcessorTest
    {
        [TestMethod]
        public void GetMethod_ProxyProcessor()
        {
            IoCContainer.Rebind(typeof(ILog), new Mock<ILog>().Object);
            const string urlToLoad = "http://stackoverflow.com/";
            var mockRequest = new Mock<ICustomHttpListenerRequest>();
            mockRequest.Setup(t => t.HttpMethod).Returns("GET");
            mockRequest.Setup(t => t.QueryString).Returns(new NameValueCollection { { "url", urlToLoad } });

            var logic = new ProxyProcessor();
            string url = logic.GetUrl(mockRequest.Object);
            Assert.AreEqual(urlToLoad, url);
        }

        [TestMethod]
        public void PostMethod_ProxyProcessor()
        {
            IoCContainer.Rebind(typeof(ILog), new Mock<ILog>().Object);
            const string urlToLoad = "http://stackoverflow.com/";
            var mockRequest = new Mock<ICustomHttpListenerRequest>();
            mockRequest.Setup(t => t.HttpMethod).Returns("POST");

            var mockLogic = new Mock<PostParamParser>();
            var dic = new Dictionary<string, string>() { { "url", urlToLoad } };
            mockLogic.Setup(t => t.Parse(mockRequest.Object)).Returns(dic);
            IoCContainer.Rebind(typeof(PostParamParser), mockLogic.Object);

            var logic = new ProxyProcessor();
            string url = logic.GetUrl(mockRequest.Object);
            Assert.AreEqual(urlToLoad, url);
        }
    }
}
