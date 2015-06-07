using System;
using System.Linq;
using System.Linq.Expressions;
using CustomWebServer.DAL.Specifications.Expressions;

namespace CustomWebServer.DAL.Specifications
{
    /// <summary>
    /// Объединение спецификаций по И
    /// </summary>
    /// <typeparam name="T">Тип объекта, для которого применяется спецификация</typeparam>
    public class AndSpecification<T> : Specification<T>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="spec">Список спецификаций</param>
        public AndSpecification(params Specification<T>[] spec)
            : base(spec.Skip(1).Aggregate((Expression<Func<T, bool>>)spec.First(), (current, specification) => current.And(specification)))
        {
        }
    }
}
