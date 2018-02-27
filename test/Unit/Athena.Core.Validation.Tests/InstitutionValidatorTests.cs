using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class InstitutionValidatorTests : UniqueObjectValidatorTest<Institution>
    {
        public InstitutionValidatorTests() : base(new InstitutionValidator())
        {
        }
        
        [Fact]
        public void Valid()
        {
            var arg = new Institution
            {
                Id = Guid.NewGuid(),
                Name = "University of Toledo",
                Description = "Senior Design II"
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void NameCannotBeNull()
        {
            var arg = _fixture.Build<Institution>()
                .WithAutoProperties()
                .With(i => i.Name, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(i => i.Name, arg);
        }

        [Fact]
        public void NameCannotBeEmpty()
        {
            var arg = _fixture.Build<Institution>()
                .WithAutoProperties()
                .With(i => i.Name, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(i => i.Name, arg);
        }

        [Fact]
        public void DescriptionCannotBeNull()
        {
            var arg = _fixture.Build<Institution>()
                .WithAutoProperties()
                .With(i => i.Description, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(i => i.Description, arg);
        }
    }
}