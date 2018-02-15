using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Athena.Core.Validation.Extensions
{
    public static class StringValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> EmailMustBeValid<T>(this IRuleBuilderInitial<T, string> builder) =>
            builder.Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must(BeAValidEmail);

        private static bool BeAValidEmail(string email)
        {
            try
            {
                var _ = new MailAddress(email).Address;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}