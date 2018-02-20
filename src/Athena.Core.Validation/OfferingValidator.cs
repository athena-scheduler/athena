using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class OfferingValidator : UniqueObjectValidator<Offering>
    {
        public OfferingValidator(IValidator<Campus> campusValidator)
        {
            RuleFor(o => o.Campus).NotNull().SetValidator(campusValidator);
            RuleFor(o => o.End).Must((o, end) => end > o.Start).WithMessage("End date cannot be before start date");
        }
    }
}