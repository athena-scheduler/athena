using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class CourseValidatorTests : UniqueObjectValidatorTest<Course>
    {
        public CourseValidatorTests() : base(new CourseValidator(new InstitutionValidator()))
        {
        }
        
        [Fact]
        public void Valid()
        {
            var arg = new Course
            {
                Id =  Guid.NewGuid(),
                Name = "EECS 4020",
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
            var arg = _fixture.Build<Course>()
                .WithAutoProperties()
                .With(c => c.Name, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Name, arg);
        }
        
        [Fact]
        public void NameCannotBeEmpty()
        {
            var arg = _fixture.Build<Course>()
                .WithAutoProperties()
                .With(c => c.Name, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Name, arg);
        }

        [Fact]
        public void InstitutionCannotBeNull()
        {
            var arg = _fixture.Build<Course>()
                .WithAutoProperties()
                .With(c => c.Institution, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Institution, arg);
        }

        [Fact]
        public void UsesInstitutionValidatorForInstitution() =>
            _sut.ShouldHaveChildValidator(c => c.Institution, typeof(InstitutionValidator));
    }
}