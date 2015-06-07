using System;
using CustomWebServer.IoC.InjectModels;
using CustomWebServer.IoC.IoCContainer;
using CustomWebServer.Logic;
using CustomWebServer.Logic.Http;
using CustomWebServer.Logic.Http.ListenerProcessor;
using log4net;
using log4net.Config;

namespace CustomWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            IoCContainer.Load(new InjectModule());
            var listenerLogic = new ListenerLogic();
            int port = IoCContainer.Get<ConfigurationLogic>().Port;
            listenerLogic.Start(new[] { String.Format("http://*:{0}/Proxy/", port) }, new ProxyProcessor());
            listenerLogic.Start(new[] { String.Format("http://*:{0}/Guestbook/", port) }, new GuestbookProcessor());
            listenerLogic.Start(new[] { String.Format("http://*:{0}/", port) }, new DefaultProcessor());
            IoCContainer.Get<ILog>().Info("Сервер успешно инициализирован.");
            Console.ReadLine();
        }

    }
}
