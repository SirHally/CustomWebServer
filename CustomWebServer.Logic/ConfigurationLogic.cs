using System;
using System.Configuration;
using CustomWebServer.Logic.Types;

namespace CustomWebServer.Logic
{
    public class ConfigurationLogic
    {
        /// <summary>
        /// Тип хранилища гостевой книги
        /// </summary>
        private  StorageType? _storageType;

        /// <summary>
        /// Тип хранилища гостевой книги
        /// </summary>
        public  StorageType Storage
        {
            get
            {
                if (!_storageType.HasValue)
                {
                    string setting = ConfigurationManager.AppSettings["StorageType"];
                    StorageType result;
                    if (!Enum.TryParse(setting, true, out result))
                    {
                        throw new ConfigurationErrorsException(String.Format("Допустимые значения настройки StorageType - xml и sql. Значение {0} недопустимо", setting));
                    }
                    _storageType = result;
                }
                return _storageType.Value;
            }
        }

        /// <summary>
        /// Прослушиваемый порт
        /// </summary>
        private  int? _port;

        /// <summary>
        /// Прослушиваемый порт
        /// </summary>
        public  int Port
        {
            get
            {
                if (!_port.HasValue)
                {
                    string setting = ConfigurationManager.AppSettings["Port"];
                    int result;
                    if (!Int32.TryParse(setting, out result))
                    {
                        throw new ConfigurationErrorsException(String.Format("Недопустимое значение для настройки Port - {0}", setting));
                    }
                    _port = result;
                }
                return _port.Value;
            }
        }
    }
}
