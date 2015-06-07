using System;
using System.Linq.Expressions;

namespace CustomWebServer.DAL.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        protected Specification(Expression<Func<T, bool>> expression)
        {
            Predicate = expression;
        }

        /// <summary>
        /// Удовлетворяет ли объект спецификации
        /// </summary>
        /// <param name="item">Проверяемый объект</param>
        public bool IsSatisfiedBy(T item)
        {
            return Predicate.Compile()(item);
        }

        /// <summary>
        /// Возвращает Expression
        /// </summary>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetPredicate()
        {
            return Predicate;
        }

        /// <summary>
        /// Предикат для проверки спецификации
        /// </summary>
        private Expression<Func<T, bool>> Predicate { get; set; }

        /// <summary>
        /// Оператор ИЛИ
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        /// <summary>
        /// Оператор И
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        /// <summary>
        /// Оператор НЕ
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static Specification<T> operator !(Specification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        public static implicit operator Predicate<T>(Specification<T> specification)
        {
            return specification.IsSatisfiedBy;
        }

        public static implicit operator Func<T, bool>(Specification<T> specification)
        {
            return specification.IsSatisfiedBy;
        }

        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.Predicate;
        }

    }
}
