using System;

namespace Athena.Core.Models
{
    public interface IUniqueObject<T> where T : IEquatable<T>
    {
        /// <summary>
        /// An identifier that uniquely identifies the object
        /// </summary>
        T Id { get; set; }
    }
}