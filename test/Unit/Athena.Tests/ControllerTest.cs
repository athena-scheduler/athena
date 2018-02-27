using Athena.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Athena.Tests
{
    public class ControllerTest
    {
        protected readonly Mock<ICampusRepository> Campuses = new Mock<ICampusRepository>();
        protected readonly Mock<ICourseRepository> Coureses = new Mock<ICourseRepository>();
        protected readonly Mock<IInstitutionRepository> Institutions = new Mock<IInstitutionRepository>();
        protected readonly Mock<IMeetingRepository> Meetings = new Mock<IMeetingRepository>();
        protected readonly Mock<IOfferingReository> Offerings = new Mock<IOfferingReository>();
        protected readonly Mock<IProgramRepository> Programs = new Mock<IProgramRepository>();
        protected readonly Mock<IRequirementRepository> Requirements = new Mock<IRequirementRepository>();
        protected readonly Mock<IStudentRepository> Students = new Mock<IStudentRepository>();
       


    }
}
