using System;
using Athena.Core.Models;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class MeetingValidatorTests : UniqueObjectValidatorTest<Meeting>
    {
        public MeetingValidatorTests() : base(new MeetingValidator())
        {
        }

        [Fact]
        public void Valid()
        {
            var arg = new Meeting
            {
                Id = Guid.NewGuid(),
                Day = DayOfWeek.Friday,
                Duration = TimeSpan.FromHours(1),
                Room = "UPRC 301",
                Time = TimeSpan.FromHours(5)
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void DurationMustBePositive()
        {
            var arg = _fixture.Build<Meeting>()
                .WithAutoProperties()
                .With(m => m.Duration, TimeSpan.FromHours(-1))
                .Create();

            _sut.ShouldHaveValidationErrorFor(m => m.Duration, arg);
        }

        [Fact]
        public void TimeMustBePositive()
        {
            var arg = _fixture.Build<Meeting>()
                .WithAutoProperties()
                .With(m => m.Time, TimeSpan.FromHours(-1))
                .Create();

            _sut.ShouldHaveValidationErrorFor(m => m.Time, arg);
        }

        [Fact]
        public void TimeMustBeLessThan24Hours()
        {
            var arg = _fixture.Build<Meeting>()
                .WithAutoProperties()
                .With(m => m.Time, TimeSpan.FromHours(24))
                .Create();

            _sut.ShouldHaveValidationErrorFor(m => m.Time, arg);
        }

        [Fact]
        public void RoomCannotBeNull()
        {
            var arg = _fixture.Build<Meeting>()
                .WithAutoProperties()
                .With(m => m.Room, null)
                .Create();

            _sut.ShouldHaveValidationErrorFor(m => m.Room, arg);
        }

        [Fact]
        public void RoomCannotBeEmpty()
        {
            var arg = _fixture.Build<Meeting>()
                .WithAutoProperties()
                .With(m => m.Room, string.Empty)
                .Create();

            _sut.ShouldHaveValidationErrorFor(m => m.Room, arg);
        }
    }
}