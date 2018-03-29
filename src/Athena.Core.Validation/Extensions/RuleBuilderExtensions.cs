using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;

namespace Athena.Core.Validation.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static void ValidateCollectionUsing<T, TCollectionElement>(
            this IRuleBuilder<T, IEnumerable<TCollectionElement>> ruleBuilder,
            Action<InlineValidator<TCollectionElement>> setup
        )
        {
            var inlineValidator = new InlineValidator<TCollectionElement>();
            setup(inlineValidator);

            var adapter = new ChildCollectionValidatorAdaptor(inlineValidator);
            ruleBuilder.SetValidator(adapter);
        }
    }
}