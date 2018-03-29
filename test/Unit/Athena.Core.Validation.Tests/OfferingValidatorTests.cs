using System;
using System.Linq;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class OfferingValidatorTests : UniqueObjectValidatorTest<Offering>
    {
        public OfferingValidatorTests() : base(new OfferingValidator(new CampusValidator(), new CourseValidator(new InstitutionValidator()), new MeetingValidator()))
        {
        }
            
        [Fact]
        public void Valid()
        {
            var arg = new Offering
            {
                Id = Guid.NewGuid(),
                Course = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = "EECS 4020",
                    Institution = new Institution
                    {
                        Id = Guid.NewGuid(),
                        Name = "The University of Toledo",
                        Description = ""
                    }
                },
                Start = DateTime.Today,
                End = DateTime.Today.AddDays(80),
                Campus = new Campus
                {
                    Id = Guid.NewGuid(),
                    Name = "UPRC",
                    Description = "University Partnership Ridge Campus",
                    Location = "32121 Lorain Rd, North Ridgeville, OH 44039"
                },
                Meetings = Enumerable.Empty<Meeting>()
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void UsesCampusValidatorForCampus() =>
            _sut.ShouldHaveChildValidator(o => o.Campus, typeof(CampusValidator));

        [Fact]
        public void UsesCourseValidatorForCourse() =>
            _sut.ShouldHaveChildValidator(o => o.Course, typeof(CourseValidator));
        
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