using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CustomWebServer.DAL.POCO;
using CustomWebServer.DAL.Repository;

namespace CustomWebServer.DAL.Entity
{
    /// <summary>
    /// Контекст для работы с данными
    /// </summary>
    public class MainContext : DbContext, IEntity
    {
        /// <summary>
        /// Имя строки подключения в конфигие
        /// </summary>
        private const string ConnectionStringName = "MainContext";

        public MainContext()
            : base(ConnectionStringName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public DbSet<Message> Message { get; set; }

        /// <summary>
        /// Получаем доступные таблицы
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private IDbSet<TEntity> Get<TEntity>() where TEntity : class
        {
            if (typeof(TEntity) == typeof(User))
            {
                return (IDbSet<TEntity>)User;
            }
            if (typeof(TEntity) == typeof(Message))
            {
                return (IDbSet<TEntity>)Message;
            }
            else
            {
                throw new NotImplementedException(String.Format("Неизвестный тип - {0}", typeof(TEntity)));
            }
        }

        /// <summary>
        /// Получаем доступ к таблице в виде IQueryable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> GetModel<TEntity>() where TEntity : class
        {
            return Get<TEntity>();
        }

        /// <summary>
        /// Добавляем новую запись
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void Add<TEntity>(TEntity item) where TEntity : class
        {
            Get<TEntity>().Add(item);
        }

        /// <summary>
        /// Удаляем запись
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void Remove<TEntity>(TEntity item) where TEntity : class
        {
            Get<TEntity>().Remove(item);
        }
    }
}
