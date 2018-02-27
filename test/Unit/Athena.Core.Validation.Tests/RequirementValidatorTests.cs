using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class RequirementValidatorTests : UniqueObjectValidatorTest<Requirement>
    {
        public RequirementValidatorTests() : base(new RequirementValidator())
        {
        }

        [Fact]
        public void Valid()
        {
            var arg = new Requirement
            {
                Id = Guid.NewGuid(),
                Name = "Calculus II",
                Description = "Multivariable Calculus"
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void NameCannotBeNull()
        {
            var arg = _fixture.Build<Requirement>()
                .WithAutoProperties()
                .With(r => r.Name, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(r => r.Name, arg);
        }

        [Fact]
        public void NameCannotBeEmpty()
        {
            var arg = _fixture.Build<Requirement>()
                .WithAutoProperties()
                .With(r => r.Name, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(r => r.Name, arg);
        }

        [Fact]
        public void DescriptionCannotBeNull()
        {
            var arg = _fixture.Build<Requirement>()
                .WithAutoProperties()
                .With(r => r.Description, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(r => r.Description, arg);
        }
    }
}