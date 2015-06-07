using System.Collections.Generic;
using System.Linq;
using CustomWebServer.DAL.Specifications;

namespace CustomWebServer.DAL.Repository
{
    public class GenericRepository<TEntity> :  IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Контекст
        /// </summary>
        protected readonly IEntity Context;

        public GenericRepository(IEntity context)
        {
            Context = context;
        }


        /// <summary>
        /// Ищем записи, удовлетворяющие условию
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(ISpecification<TEntity> criteria)
        {
            IQueryable<TEntity> dbSet = Context.GetModel<TEntity>();
            if (criteria != null)
            {
                dbSet = dbSet.Where(criteria.GetPredicate());
            }
            return dbSet;
        }

        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="element"></param>
        public void Add(TEntity element)
        {
            Context.Add(element);
        }

        /// <summary>
        /// Удаляет запись
        /// </summary>
        /// <param name="element"></param>
        public void Remove(TEntity element)
        {
            Context.Remove(element);
        }

    }
}
