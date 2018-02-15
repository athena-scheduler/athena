using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class CampusValidator : UniqueObjectValidator<Campus>
    {
        public CampusValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
            RuleFor(c => c.Description).NotNull().WithMessage("Campus Description cannot be null");
            RuleFor(c => c.Location).NotNull().NotEmpty().WithMessage("Location cannot be null or empty");
        }
    }
}