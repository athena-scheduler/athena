using System;
using System.Collections.Generic;

namespace Athena.Core.Models
{
    public class Course : IUniqueObject<Guid>
    {
        /// <inheritdoc />
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the course
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The Institution that offers this course
        /// </summary>
        public Institution Institution { get; set; }
        
        /// <summary>
        /// The requirements this course satisfies
        /// </summary>
        public List<Requirement> SatisfiesRequirements { get; } = new List<Requirement>();

        /// <summary>
        /// Any courses that must be completed before this course can be taken
        /// </summary>
        public List<Requirement> Prerequisites { get; } = new List<Requirement>();
        
        /// <summary>
        /// Any courses that are required to be taken before or concurrently with this course
        /// </summary>
        public List<Requirement> ConcurrentPrerequisites { get; } = new List<Requirement>();

        /// <summary>
        /// The dates and times this course is offerred
        /// </summary>
        public List<Offering> Offerings { get; } = new List<Offering>();
    }
}