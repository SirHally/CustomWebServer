using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using CustomWebServer.DAL.POCO;
using CustomWebServer.DAL.Repository;

namespace CustomWebServer.DAL.Entity
{
    /// <summary>
    /// Контекст для работы с XML-файлом
    /// </summary>
    public class XmlContext : IEntity
    {
        /// <summary>
        /// Имя строки подключения в конфиге
        /// </summary>
        private const string ConnectionStringName = "XmlContext";

        /// <summary>
        /// Корневой элемент
        /// </summary>
        private const string RootElement = "messages";

        /// <summary>
        /// Элемент-сообщение
        /// </summary>
        private const string ItemElement = "message";

        /// <summary>
        /// Путь к XML-файлу
        /// </summary>
        private readonly string _fileName =
            ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

        /// <summary>
        /// XML-документ
        /// </summary>
        private readonly XDocument _document;

        /// <summary>
        /// Синхронизация. Каждый новый поток будет ожидать своей очереди при доступе к файлу хранилища.
        /// </summary>
        private readonly Mutex _mutex;

        /// <summary>
        /// Поток для чтения и записи XML
        /// </summary>
        private readonly FileStream _stream;

        /// <summary>
        ///     Количество измененных, созданных, удаленных объектов
        /// </summary>
        private int _countChanges;

        public XmlContext()
        {
            const string mutexName = "xmlLocker";
            _mutex = new Mutex(false, mutexName);
            _mutex.WaitOne();
            _stream = new FileStream(_fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            _document = XDocument.Load(_stream);
            Message = (from data in _document.Descendants(RootElement).Descendants(ItemElement)
                select new Message
                {
                    Id = 0,
                    Text = data.Attribute("text").Value,
                    User = new User
                    {
                        Id = 0,
                        Name = data.Attribute("user").Value
                    }
                }).AsQueryable();

            User = (from data in _document.Descendants(RootElement).Descendants(ItemElement)
                select new User
                {
                    Id = 0,
                    Name = data.Attribute("user").Value
                }).Distinct(new UserEqulityComparer()).AsQueryable();
        }

        /// <summary>
        ///     Пользователь
        /// </summary>
        private IQueryable<User> User { get; set; }

        /// <summary>
        ///     Сообщение
        /// </summary>
        private IQueryable<Message> Message { get; set; }

        /// <summary>
        /// Освобождает заблокированный XML-файл
        /// </summary>
        public void Dispose()
        {
            _stream.Close();
            _mutex.ReleaseMutex();
        }

        /// <summary>
        /// Получаем данные определенного типа в виде IQueryable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> GetModel<TEntity>() where TEntity : class
        {
            if (typeof (TEntity) == typeof (Message))
            {
                return (IQueryable<TEntity>) Message;
            }
            if (typeof (TEntity) == typeof (User))
            {
                return (IQueryable<TEntity>) User;
            }
            throw new NotImplementedException(string.Format("Неизвестный тип - {0}", typeof (TEntity)));
        }

        /// <summary>
        /// Сохраняем изменения в XML
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            _stream.Position = 0;
            _document.Save(_stream);
            var changes = _countChanges;
            _countChanges = 0;
            return changes;
        }

        /// <summary>
        /// Добавляем запись
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void Add<TEntity>(TEntity item) where TEntity : class
        {
            if (typeof (TEntity) == typeof (Message))
            {
                _countChanges++;
                var root = _document.Descendants(RootElement).First();
                var message = item as Message;
                var element = new XElement(ItemElement);
                element.SetAttributeValue("text", message.Text);
                element.SetAttributeValue("user", message.User.Name);
                root.Add(element);
            }
            else if (typeof (TEntity) == typeof (User))
            {
                //Пользователи не хранятся отдельно и поэтому не создаются
            }
            else
            {
                throw new NotImplementedException(string.Format("Неизвестный тип - {0}", typeof (TEntity)));
            }
        }

        /// <summary>
        /// Удаляем запись
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void Remove<TEntity>(TEntity item) where TEntity : class
        {
            throw new NotImplementedException("Реализовать по мере необходимости");
        }
    }
}