using System.Collections.Generic;
using System.Linq;

namespace CustomWebServer.DAL.Specifications.POCO.User
{
    /// <summary>
    /// Спецификация по имени пользователя
    /// </summary>
    public class ByName : Specification<DAL.POCO.User>
    {
        public ByName(string value)
            : base(element => element.Name == value)
        {
        }

        public ByName(IEnumerable<string> value)
            : base(element => value.Contains(element.Name))
        {
        }
    }
}
