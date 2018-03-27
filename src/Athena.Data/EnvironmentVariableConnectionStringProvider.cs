using System;

namespace Athena.Data
{
    public class EnvironmentVariableConnectionStringProvider : IConnectionStringProvider
    {
        public static readonly string DEFAULT_CONNECTION_STRING = "Server=localhost;User ID=postgres;Database=postgres";
        public static readonly string CONNECTION_STRING_ENV = "ATHENA_CONNECTION_STRING";
        
        private static readonly Lazy<string> ConnectionString = new Lazy<string>(() =>
            Environment.GetEnvironmentVariable(CONNECTION_STRING_ENV) ?? DEFAULT_CONNECTION_STRING
        );

        public string GetConnectionString() => ConnectionString.Value;
    }
}