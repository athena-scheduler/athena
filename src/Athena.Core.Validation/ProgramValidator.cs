using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class ProgramValidator : UniqueObjectValidator<Program>
    {
        public ProgramValidator(IValidator<Institution> institutionValidator)
        {
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
            RuleFor(p => p.Description).NotNull().WithMessage("Description cannot be null");
            RuleFor(p => p.Institution).NotNull().SetValidator(institutionValidator);
        }
    }
}