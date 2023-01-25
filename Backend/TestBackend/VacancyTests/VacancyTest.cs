using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services;
using LogicLayer.Services.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TestBackend.VacancyTests
{
    [TestClass]
    public class VacancyTest
    {

        [TestMethod]
        public void FindVacancyByIdReturnFullObject()
        {
            //Setup
            Vacancy empty = new Vacancy
            {
                Id = 1,
                Location = "Stuttgard",
                MeetingLocation = "Office 342",
                Title = "German doctor"
            };
            var mock = new Mock<IVacancyRepository>();
            mock.Setup(p=> p.GetSpecificVacancy(It.IsAny<int>())).Returns(empty);
            var vacancy = new VacancyService(mock.Object);

            //Act
            vacancy.GetVacancyById(1);

            //Assert
            Assert.AreEqual(empty, vacancy.GetVacancyById(1));


        }

        [TestMethod]
        public void FindVacancyById_ObjectDoesNotExist()
        {
            //Setup
          
            var mock = new Mock<IVacancyRepository>();
            mock.Setup(p => p.GetSpecificVacancy(It.IsAny<int>()));
            var vacancy = new VacancyService(mock.Object);

            //Act
            vacancy.GetVacancyById(2);

            //Assert
            Assert.AreEqual(null, vacancy.GetVacancyById(1));


        }

        [TestMethod]
        public void FindVacancyByIdIntergrationTest()
        {
            //Setup
            var mock = new Mock<IVacancyRepository>();
            mock.Setup(p => p.GetSpecificVacancy(It.IsAny<int>()));
            var vacancy = new VacancyService(mock.Object);

            //Act
            vacancy.GetVacancyById(1);

            //Assert
            mock.Verify(p => p.GetSpecificVacancy(It.IsAny<int>()), Times.Once);

        }

        [TestMethod]
        public void FindAllVacanciesTest()
        {
            //Setup
            var mock = new Mock<IVacancyRepository>();
            mock.Setup(p => p.GetAllVacancies())
                .Returns(new List<Vacancy>
                {
                    new Vacancy
                    {
                        Id = 1,
                        MeetingLocation = "Online",
                        Location = "Aether",
                        Title = "IT Professional"
                    }
                });
            var vacancy = new VacancyService(mock.Object);

            //Act
            vacancy.GetAllVacancies();

            //Assert
            Assert.AreEqual(1, vacancy.GetAllVacancies().Count());
        }

      



        [TestMethod]
        public void FindAllVacanciesIntergrationTest()
        {
            //Setup
            var mock = new Mock<IVacancyRepository>();
            mock.Setup(p => p.GetAllVacancies());
            var vacancy = new VacancyService(mock.Object);

            //Act
            vacancy.GetAllVacancies();

            //Assert
            Assert.IsFalse(mock.Equals(Times.Once));
        }
    }
}
