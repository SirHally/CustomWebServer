using System;
using System.Collections.Generic;
using CustomWebServer.DAL.POCO;

namespace CustomWebServer.DAL.Entity
{
    internal class UserEqulityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return string.Equals(x.Name, y.Name, StringComparison.Ordinal);
        }

        public int GetHashCode(User obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
