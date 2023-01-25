using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogicLayer.Repositories;
using LogicLayer.Models;
using LogicLayer.Services.Classes;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TestBackend.ApplicationTests
{
    [TestClass]
    public class ApplicationTest
    {

        [TestMethod]
        public void GetApplicationssByVacancyIntergrationTest()
        {
            //Setup
            var mock = new Mock<IApplicationRepository>();
            mock.Setup(p => p.GetApplicationssByVacancy(1));
            var application = new ApplicationService(mock.Object);

            //Act
            application.GetApplicationsByVacancy(1);

            //Assert
            mock.Verify(p => p.GetApplicationssByVacancy(1), Times.Once);

        }

        [TestMethod]
        public void GetApplicationssByVacancyTest()
        {
            //Setup
            User u = new User { Id = 1, };
            Vacancy vacancy = new Vacancy
            {
                Id = 1
            };

            Application appy = new Application
            {
                Id = 1,
                Candidate = u,
                JobVacancy = vacancy

            };
            var mock = new Mock<IApplicationRepository>();
            mock.Setup(p => p.GetApplicationssByVacancy(1))
               .Returns(new List<Application> { appy });
            var application = new ApplicationService(mock.Object);



            //Assert
            Assert.AreEqual(1, application.GetApplicationsByVacancy(1).Count());

        }

        [TestMethod]
        public void GetApplicationssByVacancyFalse()
        {
            //Setup
            var mock = new Mock<IApplicationRepository>();
            mock.Setup(p => p.GetApplicationssByVacancy(0));
            var application = new ApplicationService(mock.Object);

            //Act
            application.GetApplicationsByVacancy(0);

            //Assert
            mock.Verify(p => p.GetApplicationssByVacancy(0), Times.Once);

        }

        [TestMethod]
        public void InsertApplicationIntergration()
        {
            Application application = new Application();
            application.Id = 1;

            var mock = new Mock<IApplicationRepository>();
            mock.Setup(p => p.InsertApplication(application));
            var app = new ApplicationService(mock.Object);

            app.InsertApplication(application);

            mock.Verify(p => p.InsertApplication(application), Times.Once);

        }
     




    }
}
