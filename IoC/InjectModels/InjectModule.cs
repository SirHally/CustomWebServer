using System;
using System.Linq;
using CustomWebServer.DAL.Entity;
using CustomWebServer.DAL.Repository;
using CustomWebServer.Logic;
using CustomWebServer.Logic.Types;
using log4net;
using Ninject.Modules;

namespace CustomWebServer.IoC.InjectModels
{
    /// <summary>
    ///     Это общие биндинги Ninject 
    /// </summary>
    public class InjectModule : NinjectModule
    {
        public override void Load()
        {
            var configurationLogic = new ConfigurationLogic();

            switch (configurationLogic.Storage)
            {
                case StorageType.Xml:
                    Bind<IEntity>().To<XmlContext>();
                    break;
                case StorageType.Sql:
                    Bind<IEntity>().To<MainContext>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Неизвестное значение перечисления - " + configurationLogic.Storage);
            }
            
            // биндинг логера
            Bind<ILog>().ToConstant(LogManager.GetLogger("MainLogger"));

            //Репозитарии
            BindRepository();

            //Конфигурация
            Bind<ConfigurationLogic>().ToConstant(configurationLogic).InSingletonScope();

        }

        /// <summary>
        /// Биндим репозитарии
        /// </summary>
        private void BindRepository()
        {
            Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
        }


    }
}