using System;
using Athena.Core.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace Athena.Core.Validation.Tests
{
    public class StudentValidatorTests : UniqueObjectValidatorTest<Student>
    {
        public StudentValidatorTests() : base(new StudentValidator())
        {
        }

        [Fact]
        public void Valid()
        {
            var arg = new Student
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "jdoe2@rockets.utoledo.edu"
            };
            
            Assert.True(_sut.Validate(arg).IsValid);
        }

        [Fact]
        public void NameCannotBeNull()
        {
            var arg = new Student
            {
                Id = Guid.NewGuid(),
                Name = null,
                Email = "jdoe2@rockets.utoledo.edu"
            };

            _sut.ShouldHaveValidationErrorFor(s => s.Name, arg);
        }
        
        [Fact]
        public void NameCannotBeEmpty()
        {
            var arg = new Student
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Email = "jdoe2@rockets.utoledo.edu"
            };

            _sut.ShouldHaveValidationErrorFor(s => s.Name, arg);
        }

        [Fact]
        public void EmailCannotBeNull()
        {
            var arg = new Student
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = null
            };

            _sut.ShouldHaveValidationErrorFor(s => s.Email, arg);
        }
        
        [Fact]
        public void EmailMustBeValid()
        {
            var arg = new Student
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "foo@foo@foo.com"
            };

            _sut.ShouldHaveValidationErrorFor(s => s.Email, arg);
        }
    }
}