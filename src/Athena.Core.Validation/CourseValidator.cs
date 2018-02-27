using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class CourseValidator : UniqueObjectValidator<Course>
    {
        public CourseValidator(IValidator<Institution> institutionValidator)
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("Course name cannot be null or empty");
            RuleFor(c => c.Institution).NotNull().SetValidator(institutionValidator);
        }
    }
}