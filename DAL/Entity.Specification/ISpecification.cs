using System;
using System.Linq.Expressions;

namespace CustomWebServer.DAL.Specifications
{
    /// <summary>
    /// Интерфейс спецификации
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Удовлетворяет ли объект спецификации
        /// </summary>
        /// <param name="item">Проверяемый объект</param>
        bool IsSatisfiedBy(T item);
        /// <summary>
        /// Возвращает Expression
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> GetPredicate();
    }
}
