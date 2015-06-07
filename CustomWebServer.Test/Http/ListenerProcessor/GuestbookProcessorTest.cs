using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using CustomWebServer.DAL.POCO;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic;
using CustomWebServer.Logic.Http;
using CustomWebServer.Logic.Http.ListenerProcessor;
using CustomWebServer.Logic.Types;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace CustomWebServer.Test.Http.ListenerProcessor
{
    [TestClass]
    public class GuestbookProcessorTest
    {
        [TestMethod]
        public void GetAll_GuestbookProcessor()
        {
            IoCContainer.Rebind(typeof(ILog), new Mock<ILog>().Object);
            var mockRequest = new Mock<ICustomHttpListenerRequest>();
            mockRequest.Setup(t => t.HttpMethod).Returns("GET");

            var mockResponse = new Mock<ICustomHttpListenerResponse>();

            var mockMessage = new MessageResult()
            {
                User = "sirhally",
                Text = "Hello!"
            };
            var collection = new List<MessageResult> {mockMessage};
            string json = JsonConvert.SerializeObject(collection);
            var mockLogic = new Mock<GuestbookLogic>();
            mockLogic.Setup(t => t.GetAll()).Returns(collection);
            IoCContainer.Rebind(typeof(GuestbookLogic), mockLogic.Object);

            var resposeLogic = new Mock<ResponseLogic>();
            resposeLogic.Setup(t => t.Execute(It.IsAny<string>(), It.IsAny<ICustomHttpListenerResponse>()));
            IoCContainer.Rebind(typeof(ResponseLogic), resposeLogic.Object);

            var logic = new GuestbookProcessor();
            logic.Process(mockRequest.Object, mockResponse.Object);

            mockLogic.Verify(t=>t.GetAll(),Times.Once);
            mockResponse.VerifySet(t => t.ContentType = "application/json", Times.Once());
            resposeLogic.Verify(t => t.Execute(json, It.IsAny<ICustomHttpListenerResponse>()), Times.Once);

        }

        [TestMethod]
        public void PostCreate_GuestbookProcessor()
        {
            IoCContainer.Rebind(typeof(ILog), new Mock<ILog>().Object);

            var mockRequest = new Mock<ICustomHttpListenerRequest>();
            mockRequest.Setup(t => t.HttpMethod).Returns("POST");

            var mockParser = new Mock<PostParamParser>();
            mockParser.Setup(t => t.Parse(mockRequest.Object)).Returns(new Dictionary<string, string>()
            {
                {"user", "sirhally"},
                {"message", "Hello"}
            });
            IoCContainer.Rebind(typeof(PostParamParser), mockParser.Object);

            var mockResponse = new Mock<ICustomHttpListenerResponse>();

            var mockLogic = new Mock<GuestbookLogic>();
            mockLogic.Setup(t => t.CreateMessage(It.IsAny<string>(), It.IsAny<string>()));
            IoCContainer.Rebind(typeof(GuestbookLogic), mockLogic.Object);

            var resposeLogic = new Mock<ResponseLogic>();
            resposeLogic.Setup(t => t.Execute(It.IsAny<string>(), It.IsAny<ICustomHttpListenerResponse>()));
            IoCContainer.Rebind(typeof(ResponseLogic), resposeLogic.Object);

            var logic = new GuestbookProcessor();
            logic.Process(mockRequest.Object, mockResponse.Object);

            mockLogic.Verify(t => t.CreateMessage("sirhally", "Hello"), Times.Once);
            resposeLogic.Verify(t => t.Execute(String.Empty, It.IsAny<ICustomHttpListenerResponse>()), Times.Once);
        }
    }
}
