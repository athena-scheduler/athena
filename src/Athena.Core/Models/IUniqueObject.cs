using System;

namespace Athena.Core.Models
{
    public interface IUniqueObject<T> where T : IEquatable<T>
    {
        T Id { get; set; }
    }
}