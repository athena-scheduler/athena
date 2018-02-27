using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class CampusValidatorTests : UniqueObjectValidatorTest<Campus>
    {
        public CampusValidatorTests() : base(new CampusValidator())
        {
        }

        [Fact]
        public void Valid()
        {
            var arg = new Campus
            {
                Id = Guid.NewGuid(),
                Name = "UPRC",
                Description = "University Partnership Ridge Campus",
                Location = "32121 Lorain Rd, North Ridgeville, OH 44039"
            };

            Assert.True(_sut.Validate(arg).IsValid);
        }
        
        [Fact]
        public void NameCannotBeNull()
        {
            var arg = _fixture.Build<Campus>()
                .WithAutoProperties()
                .With(c => c.Name, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Name, arg);
        }

        [Fact]
        public void NameCannotBeEmpty()
        {
            var arg = _fixture.Build<Campus>()
                .WithAutoProperties()
                .With(c => c.Name, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Name, arg);
        }

        [Fact]
        public void DescriptionCannotBeNull()
        {
            var arg = _fixture.Build<Campus>()
                .WithAutoProperties()
                .With(c => c.Description, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Description, arg);
        }

        [Fact]
        public void LocationCannotBeNull()
        {
            var arg = _fixture.Build<Campus>()
                .WithAutoProperties()
                .With(c => c.Location, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Location, arg);
        }
        
        [Fact]
        public void LocationCannotBeEmpty()
        {
            var arg = _fixture.Build<Campus>()
                .WithAutoProperties()
                .With(c => c.Location, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(c => c.Location, arg);
        }
    }
}