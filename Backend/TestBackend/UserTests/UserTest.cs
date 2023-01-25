using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogicLayer.Repositories;
using LogicLayer.Models;
using LogicLayer.Services.Classes;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TestBackend.UserTests
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void FindAllCandidatesByRoleIntergration()
        {
            //Setup
            var mock = new Mock<IUserRepository>();
            mock.Setup(p => p.FindAllCandidatesByRole());
            var user = new UserService(mock.Object);

            //Act
            user.can();

            //Assert
            mock.Verify(p => p.FindAllCandidatesByRole(), Times.Once);

        }

        [TestMethod]
        public void FindAllCandidatesByRoleTest()
        {
            //Setup
            var mock = new Mock<IUserRepository>();
            mock.Setup(p=> p.FindAllCandidatesByRole())
                .Returns(new List<User>
                {
                    new User {Id = 1, FirstName = "Ben", LastName = "Potter", Email = "Yeet", Password="2" }
                });
            var user = new UserService(mock.Object);
            
            Assert.AreEqual(1, user.can().Count());
        }

    

      

        
    }
}
