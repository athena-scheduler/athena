using System.Collections.Generic;
using System.Net;
using Athena.Core.Models;
using Athena.Models;

namespace Athena.Exceptions
{
    public class UnmetDependencyException : ApiException
    {
        public UnmetDependencyException(Course course, List<Requirement> unmet, List<Requirement> unmetConcurrent)
            : base(
                HttpStatusCode.PreconditionFailed,
                "Attempted to enroll in a course without meeting all requirement conditions",
                new
                {
                    course,
                    unmet,
                    unmetConcurrent
                }
            )
        {
        }
    }
}