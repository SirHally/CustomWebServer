namespace CustomWebServer.DAL.Specifications
{
    /// <summary>
    /// Спецификация, которая всегда возвращает истину
    /// </summary>
    public class TrueSpecification<T> : Specification<T>
    {
        public TrueSpecification() :
            base(element => true)
        {
        }
    }
}
