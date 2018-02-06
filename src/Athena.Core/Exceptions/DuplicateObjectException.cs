using System;

namespace Athena.Core.Exceptions
{
    public class DuplicateObjectException : ArgumentException
    {
        private readonly string Query;
        private readonly object Args;
        
        public DuplicateObjectException(string query, object args) : base("Tried to insert a duplicate object")
        {
            Query = query;
            Args = args;
        }
    }
}