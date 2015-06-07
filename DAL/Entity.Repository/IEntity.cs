using System;
using System.Data.Entity;
using System.Linq;

namespace CustomWebServer.DAL.Repository
{
    public interface IEntity:IDisposable
    {
        /// <summary>
        /// Получаем предикат для запроса
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity> GetModel<TEntity>() where TEntity : class;

        /// <summary>
        /// Сохраняем изменения
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Добавляем новый элемент
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        void Add<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Удаляем элемент
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        void Remove<TEntity>(TEntity item) where TEntity : class;
    }
}
