using System.Net;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic.Http;
using CustomWebServer.Logic.Http.ListenerProcessor;
using CustomWebServer.Logic.Types;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CustomWebServer.Test.Http.ListenerProcessor
{
    /// <summary>
    /// Тесты на DefaultProcessor
    /// </summary>
    [TestClass]
    public class DefaultProcessorTest
    {
        /// <summary>
        /// Тестируе обработку запроса Process
        /// </summary>
        [TestMethod]
        public void DefaultProcessor_Process()
        {
            var logic = new DefaultProcessor();
            var parseLogic = new Mock<ResponseLogic>();
            parseLogic.Setup(t => t.Execute(It.IsAny<string>(), It.IsAny<ICustomHttpListenerResponse>()));

            IoCContainer.Rebind(typeof(ILog), new Mock<ILog>().Object);
            IoCContainer.Rebind(typeof(ResponseLogic), parseLogic.Object);

            logic.Process(new Mock<ICustomHttpListenerRequest>().Object, new Mock<ICustomHttpListenerResponse>().Object);

            parseLogic.Verify(t => t.Execute(DefaultProcessor.ResponseString, It.IsAny<ICustomHttpListenerResponse>()), Times.Once);
        }
    }
}
