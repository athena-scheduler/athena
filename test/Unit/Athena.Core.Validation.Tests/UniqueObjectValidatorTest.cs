using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public abstract class UniqueObjectValidatorTest<T> where T: class, IUniqueObject<Guid>
    {
        protected readonly Fixture _fixture = new Fixture();
        protected readonly IValidator<T> _sut;

        public UniqueObjectValidatorTest(IValidator<T> sut) => _sut = sut;

        [Fact]
        public void IdCannotBeEmptyGuid()
        {
            var arg = _fixture.Build<T>()
                .WithAutoProperties()
                .With(t => t.Id, Guid.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(t => t.Id, arg);
        }
    }
}