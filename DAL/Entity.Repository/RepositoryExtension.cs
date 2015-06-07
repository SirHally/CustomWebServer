using CustomWebServer.IoC.IoCContainer;

namespace CustomWebServer.DAL.Repository
{
    public static class RepositoryExtension 
    {
        /// <summary>
        /// Получаем репозитарий для конкретного контекста
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TRepository GetRepository<TRepository>(this IEntity context) where TRepository : class
        {
            return IoCContainer.Get<TRepository>(ParamType.ConstructorArgiment, "context", context);
        }
    }
}
