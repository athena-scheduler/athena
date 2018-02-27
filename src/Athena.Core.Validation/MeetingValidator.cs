using System;
using Athena.Core.Models;
using FluentValidation;

namespace Athena.Core.Validation
{
    public class MeetingValidator : UniqueObjectValidator<Meeting>
    {
        public MeetingValidator()
        {
            RuleFor(m => m.Duration).Must(d => d > TimeSpan.Zero).WithMessage("Duration must be positive");
            RuleFor(m => m.Time).Must(t => t >= TimeSpan.Zero).Must(t => t < TimeSpan.FromHours(24))
                .WithMessage("Time must be between 0:00 and 23:59");
            RuleFor(m => m.Room).NotNull().NotEmpty().WithMessage("Room cannot be null or empty");
        }
    }
}