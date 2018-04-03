using Athena.Core.Models;
using Athena.Core.Validation.Extensions;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class OfferingValidator : UniqueObjectValidator<Offering>
    {
        public OfferingValidator(IValidator<Campus> campusValidator, IValidator<Course> courseValidator, IValidator<Meeting> meetingValidator)
        {
            RuleFor(o => o.Campus).NotNull().SetValidator(campusValidator);
            RuleFor(o => o.Course).NotNull().SetValidator(courseValidator);
            RuleFor(o => o.End).Must((o, end) => end > o.Start).WithMessage("End date cannot be before start date");
            RuleFor(o => o.Meetings).NotNull().ValidateCollectionUsing(m => m.Include(meetingValidator));
        }
    }
}