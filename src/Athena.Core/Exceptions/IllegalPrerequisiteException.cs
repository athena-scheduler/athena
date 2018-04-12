using System;
using Athena.Core.Models;

namespace Athena.Core.Exceptions
{
    public class IllegalPrerequisiteException : ArgumentException
    {
        public readonly Course Course;
        public readonly Requirement Prereq;

        public IllegalPrerequisiteException(Course course, Requirement prereq) :
            base("A requirement cannot be both a prerequisite and a concurrent prerequisite")
        {
            Course = course;
            Prereq = prereq;
        }
    }
}