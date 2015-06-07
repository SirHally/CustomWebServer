using System.Collections.Generic;
using CustomWebServer.DAL.Specifications;

namespace CustomWebServer.DAL.Repository
{
    public interface IRepository<TEntity>
    {

        /// <summary>
        /// Ищем записи удовлетворяющие условию
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> Find(ISpecification<TEntity> criteria);

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="element"></param>
        void Add(TEntity element);

        /// <summary>
        /// Удаляет запись
        /// </summary>
        /// <param name="element"></param>
        void Remove(TEntity element);
    }
}
