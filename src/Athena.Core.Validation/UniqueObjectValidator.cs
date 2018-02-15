using System;
using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class UniqueObjectValidator<T> : AbstractValidator<T> where T: IUniqueObject<Guid>
    {
        protected UniqueObjectValidator()
        {
            RuleFor(c => c.Id).NotEqual(Guid.Empty).WithMessage("ID cannot be empty");
        }
    }
}