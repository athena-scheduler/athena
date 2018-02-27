using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class OfferingValidatorTests : UniqueObjectValidatorTest<Offering>
    {
        public OfferingValidatorTests() : base(new OfferingValidator(new CampusValidator()))
        {
        }
            
        [Fact]
        public void Valid()
        {
            var arg = new Offering
            {
                Id = Guid.NewGuid(),
                Start = DateTime.Today,
                End = DateTime.Today.AddDays(80),
                Campus = new Campus
                {
                    Id = Guid.NewGuid(),
                    Name = "UPRC",
                    Description = "University Partnership Ridge Campus",
                    Location = "32121 Lorain Rd, North Ridgeville, OH 44039"
                }
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void UsesCampusValidatorForCampus() =>
            _sut.ShouldHaveChildValidator(o => o.Campus, typeof(CampusValidator));

        [Fact]
        public void EndDateCannotBeBeforeStartDate()
        {
            var arg = _fixture.Build<Offering>()
                .WithAutoProperties()
                .With(o => o.Start, DateTime.Today)
                .With(o => o.End, DateTime.Today.AddDays(-80))
                .Create();

            _sut.ShouldHaveValidationErrorFor(o => o.End, arg);
        }
    }
}