using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class InstitutionValidator : UniqueObjectValidator<Institution>
    {
        public InstitutionValidator()
        {
            RuleFor(i => i.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
            RuleFor(i => i.Description).NotNull().WithMessage("Description cannot be null");
        }
    }
}