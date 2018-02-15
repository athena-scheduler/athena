using Athena.Core.Models;
using Athena.Core.Validation.Extensions;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class StudentValidator : UniqueObjectValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(s => s.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
            RuleFor(s => s.Email).EmailMustBeValid().WithMessage("Invalid Email Address");
        }
    }
}