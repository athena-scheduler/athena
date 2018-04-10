using System;

namespace Athena.Core.Exceptions
{
    public class DependentObjectException : ArgumentException
    {
        private readonly string Query;
        private readonly object Args;
        
        public DependentObjectException(string query, object args) : base("An object in the relationship graph has not been created yet")
        {
            Query = query;
            Args = args;
        }
    }
}