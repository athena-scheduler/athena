using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class ProgramValidatorTests : UniqueObjectValidatorTest<Program>
    {
        public ProgramValidatorTests() : base(new ProgramValidator(new InstitutionValidator()))
        {
        }

        [Fact]
        public void Valid()
        {
            var arg = new Program
            {
                Id = Guid.NewGuid(),
                Name = "CSE",
                Description = "Computer Science & Engineering",
                Institution = new Institution
                {
                    Id = Guid.NewGuid(),
                    Name = "University of Toledo",
                    Description = "Senior Design II"
                }
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void NameCannotBeNull()
        {
            var arg = _fixture.Build<Program>()
                .WithAutoProperties()
                .With(p => p.Name, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(p => p.Name, arg);
        }

        [Fact]
        public void NameCannotBeEmpty()
        {
            var arg = _fixture.Build<Program>()
                .WithAutoProperties()
                .With(p => p.Name, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(p => p.Name, arg);
        }

        [Fact]
        public void DescriptionCannotBeNull()
        {
            var arg = _fixture.Build<Program>()
                .WithAutoProperties()
                .With(p => p.Description, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(p => p.Description, arg);
        }

        [Fact]
        public void UsesInstitutionValidatorForInstitution() =>
            _sut.ShouldHaveChildValidator(p => p.Institution, typeof(InstitutionValidator));
    }
}