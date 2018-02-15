using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class RequirementValidator : UniqueObjectValidator<Requirement>
    {
        public RequirementValidator()
        {
            RuleFor(r => r.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
            RuleFor(r => r.Description).NotNull().WithMessage("Description cannot be null");
        }
    }
}