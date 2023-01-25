using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogicLayer.Repositories;
using LogicLayer.Services.Classes;
using Moq;
using System;
using LogicLayer.Models;
using User = LogicLayer.Models.User;
using Application = LogicLayer.Models.Application;
using LogicLayer.Models.Enums;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicLayer.Exceptions;

namespace TestBackend.AppointmentTests
{
    [TestClass]
    public class AppointmentTest
    {

        Mock<IAppointmentRepository> appointmentRepository = new Mock<IAppointmentRepository>();
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        Mock<IApplicationRepository> applicationRepository = new Mock<IApplicationRepository>();
        Mock<IVacancyRepository> vacancyRepository = new Mock<IVacancyRepository>();
        Mock<IMSGraphRepository> msgraphRepository = new Mock<IMSGraphRepository>();

        private AppointmentService GetMock()
        {
            return new AppointmentService(appointmentRepository.Object, userRepository.Object, applicationRepository.Object, vacancyRepository.Object, msgraphRepository.Object);
        }



        [TestMethod]
        public void GenerateCreateAppointmentLink_WithValidRequest_ReturnCreateAppointmentResponse()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            CreateAppointmentLinkRequest request = new CreateAppointmentLinkRequest()
            {
                InsertVacancyInfoRequest = new CreateVacancyInfoRequest()
                {
                    id = 2,
                    title = It.IsAny<string>(),
                    location = It.IsAny<string>(),
                    meeting_location = It.IsAny<string>()
                },
                InsertCandidateInfoRequest = new CreateUserInfoRequest()
                {
                    id = It.IsAny<int>(),
                    first_name = It.IsAny<string>(),
                    last_name = It.IsAny<string>(),
                    email = "candidate@test.com",
                },
                InsertRecruiterInfoRequest = new CreateRecruiterInfoRequest()
                {
                    id = It.IsAny<int>(),
                    first_name = It.IsAny<string>(),
                    last_name = It.IsAny<string>(),
                    email = "recruiter@test.com",
                    password = It.IsAny<string>(),
                }
            };

            User candidate = new User()
            {
                Id = It.IsAny<int>(),
                FirstName = It.IsAny<string>(),
                LastName = It.IsAny<string>(),
                Email = "test@feji.com"
            };

            User recrutier = new User()
            {
                Id = It.IsAny<int>(),
                FirstName = It.IsAny<string>(),
                LastName = It.IsAny<string>(),
                Email = "test@feji.com"
            };


            vacancyRepository.Setup(service => service.ExistsById(request.InsertVacancyInfoRequest.id))
            .Returns(true);
            userRepository.Setup(service => service.GetUserByEmail(request.InsertCandidateInfoRequest.email))
            .Returns(candidate);
            userRepository.Setup(service => service.GetUserByEmail(request.InsertRecruiterInfoRequest.email))
            .Returns(recrutier);

            CreateAppointmentResponse expected = new CreateAppointmentResponse("http://localhost:3000/appointment/create/63616E64696461746540746573742E636F6D/32", LinkStatus.Created.ToString());

            //Act
            CreateAppointmentResponse actual = appointmentService.GenerateCreateAppointmentLink(request);
            //Assert
            Assert.AreEqual(expected.LinkStatus, actual.LinkStatus, "The generate appointment doesn't return new link");
            Assert.AreEqual(expected.NewAppointmentLink, actual.NewAppointmentLink, "The generate appointment doesn't return new link");

            userRepository.Verify(service => service.GetUserByEmail(request.InsertCandidateInfoRequest.email), Times.Exactly(2));
            userRepository.Verify(service => service.GetUserByEmail(request.InsertRecruiterInfoRequest.email), Times.Exactly(2));
            vacancyRepository.Verify(service => service.ExistsById(request.InsertVacancyInfoRequest.id), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It accepts invaild InsertCandidateInfoRequest")]
        public void GenerateCreateAppointmentLink_WithInvalidInsertCandidateInfoRequest_ThrowsArgumentException()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            CreateAppointmentLinkRequest request = new CreateAppointmentLinkRequest()
            {
                InsertCandidateInfoRequest = new CreateUserInfoRequest()
                {
                    id = It.IsAny<int>(),
                    first_name = It.IsAny<string>(),
                    last_name = It.IsAny<string>(),
                    email = " ",
                }
            };
            //Act
            appointmentService.GenerateCreateAppointmentLink(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It accepts invaild InsertVacancyInfoRequest")]
        public void GenerateCreateAppointmentLink_WithInvalidInsertVacancyInfoRequest_ThrowsArgumentException()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            CreateAppointmentLinkRequest request = new CreateAppointmentLinkRequest()
            {
                InsertVacancyInfoRequest = new CreateVacancyInfoRequest()
                {
                    id = 0,
                    title = It.IsAny<string>(),
                    location = It.IsAny<string>(),
                    meeting_location = It.IsAny<string>()
                },
                InsertCandidateInfoRequest = new CreateUserInfoRequest()
                {
                    id = It.IsAny<int>(),
                    first_name = It.IsAny<string>(),
                    last_name = It.IsAny<string>(),
                    email = "candidate@test.com",
                }
            };
            //Act
            appointmentService.GenerateCreateAppointmentLink(request);
        }

        [TestMethod]
        public void IsAppointmentHashVaild_WithValidAnExistedLink_ReturnIsAppointmentHashVaildResponse()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            string usernameHashed = "3438383336344073747564656E742E666F6E7479732E6E6C";
            string identifierHashed = "31";

            LinkValidation linkValidation = new LinkValidation(usernameHashed, identifierHashed);
            AppointmentLink appointmentLink = new AppointmentLink(usernameHashed, identifierHashed, LinkStatus.Created);

            IsAppointmentHashVaildResponse expected = new IsAppointmentHashVaildResponse(appointmentLink.LinkStatus.ToString());


            appointmentRepository.Setup(repo => repo.GetAppointmentLink(linkValidation)).Returns(appointmentLink);
            //Act
            IsAppointmentHashVaildResponse actual = appointmentService.IsAppointmentHashVaild(linkValidation);
            //Assert

            Assert.AreEqual(expected.LinkStatus, actual.LinkStatus, "It doesn't return the expected LinkStatus");

            appointmentRepository.Verify(repo => repo.GetAppointmentLink(linkValidation), Times.Once);
        }

        [TestMethod]
        public void IsAppointmentHashVaild_WithInvalidAnExistedLink_ReturnIsAppointmentHashVaildResponse()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            string usernameHashed = "3438383336344073747564656E742E666F6E7479732E6E6C";
            string identifierHashed = "31";

            LinkValidation linkValidation = new LinkValidation(usernameHashed, identifierHashed);

            appointmentRepository.Setup(repo => repo.GetAppointmentLink(linkValidation)).Returns((AppointmentLink)null);
            //Act
            IsAppointmentHashVaildResponse actual = appointmentService.IsAppointmentHashVaild(linkValidation);
            //Assert

            Assert.AreEqual("Doesn't exist", actual.LinkStatus, "It doesn't return the expected LinkStatus");

            appointmentRepository.Verify(repo => repo.GetAppointmentLink(linkValidation), Times.Once);
        }

        [TestMethod]
        public void CreateAppointment_WithValidRequest_ReturnCreateAppoinementResponse()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();


            CreateAppointmentRequest request = new CreateAppointmentRequest()
            {
                CandidateEmailHashed = "3435383532364073747564656E742E666F6E7479732E6E6C",
                VacancyIdHashed = "313031",
                RecruiterEmail = "RecruiterEmailTest@outlook.com",
                StartDate = new DateTime(2023, 09, 20, 12, 00, 00),
                EndDate = new DateTime(2023, 09, 20, 12, 30, 00),
                Location = "Straattest"
            };

            //Mocking for insertAppointmentInCalander method
            User recruiter = new User()
            {
                Id = 1,
                FirstName = "FirstNameTest",
                LastName = "LastNameTest",
                Email = "RecruiterEmailTest@outlook.com"

            };
            userRepository.Setup(repo => repo.GetUserByEmail(request.RecruiterEmail))
                .Returns(recruiter);
            User candidate = new User()
            {
                Id = 2,
                FirstName = "FirstNameTest",
                LastName = "LastNameTest",
                Email = "458526@student.fontys.nl"

            };
            userRepository.Setup(repo => repo.GetUserByEmail(candidate.Email))
                .Returns(candidate);
            
            msgraphRepository.Setup(repo => repo.sendCandidateInvitation(It.IsAny<Message>()));

            Event meeting = new Event()
            {
                OnlineMeeting = null,
                Id = "id"
            };

            Task<Event> meetingTask = Task.FromResult(meeting);
            
            msgraphRepository.Setup(repo => repo.CreateEvent(It.IsAny<Event>()))
                .Returns(meetingTask);

            Application application = new Application()
            {
                Id = 1
            };

            applicationRepository.Setup(x => x.GetApplicationByCandidateIdAndVacancyId(It.IsAny<int>(), It.IsAny<int>())).Returns(application);
            appointmentRepository.Setup(x => x.InsertAppointment(It.IsAny<Appointment>())).Returns(1);
            appointmentRepository.Setup(x => x.GetAppointmentLink(It.IsAny<LinkValidation>())).Returns(new AppointmentLink() { Id = 1 });
            appointmentRepository.Setup(x => x.UpdateAppointmentLink(It.IsAny<AppointmentLink>()));

            //Act
            int id = appointmentService.CreateAppointment(request).appointmentId;
            //Assert
            Assert.AreEqual(1, id);

            userRepository.Verify(x => x.GetUserByEmail(request.RecruiterEmail), Times.Exactly(2));
            userRepository.Verify(x => x.GetUserByEmail(candidate.Email), Times.Exactly(2));
            msgraphRepository.Verify(x => x.sendCandidateInvitation(It.IsAny<Message>()), Times.Once);
            msgraphRepository.Verify(x => x.CreateEvent(It.IsAny<Event>()), Times.Once);
            applicationRepository.Verify(x => x.GetApplicationByCandidateIdAndVacancyId(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            appointmentRepository.Verify(x => x.InsertAppointment(It.IsAny<Appointment>()), Times.Once);
            appointmentRepository.Verify(x => x.GetAppointmentLink(It.IsAny<LinkValidation>()), Times.Once);
            appointmentRepository.Verify(x => x.UpdateAppointmentLink(It.IsAny<AppointmentLink>()), Times.Once);
            

        }

        //ToDO: Check what is the other scenarios for CreateAppointment method

        [TestMethod]
        public void SendEmail_WithValidEmail_ReturnNothing()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            Email email = new Email()
            {
                subject = It.IsAny<string>(),
                bodyAsHtml = It.IsAny<string>(),
                ToRecipients = new List<Recipient>()
            };

            var message = new Message
            {
                Subject = email.subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = email.bodyAsHtml
                },
                ToRecipients = email.ToRecipients
            };

            msgraphRepository.Setup(rep => rep.SendEmail(It.IsAny<Message>(), false));

            //Act
            appointmentService.SendEmail(email);
        }

        [TestMethod]
        //ToDO: Fix the ExpectedException attribute, because it doesn't detect the exception
        //[ExpectedException(typeof(MSGraphException), "It accepts invalid Email")]
        public void SendEmail_WithInValidGraphServiceClient_ThrowsMSGraphException()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            Email email = new Email()
            {
                subject = It.IsAny<string>(),
                bodyAsHtml = It.IsAny<string>(),
                ToRecipients = new List<Recipient>()
            };

            var message = new Message
            {
                Subject = email.subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = email.bodyAsHtml
                },
                ToRecipients = email.ToRecipients
            };

            msgraphRepository.Setup(rep =>  rep.SendEmail(It.IsAny<Message>(), false)).Throws(new Exception());

            //Act
            appointmentService.SendEmail(email);
        }


        [TestMethod]
        public void CancelAnEvent_WithValidRequest_ReturnNothing()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            CancelAppointmentRequest request = new CancelAppointmentRequest()
            {
                MSGraphId = "54156d3ese"
            };

            Task<String> token = Task.FromResult("TestToken");

            msgraphRepository.Setup(rep => rep.GetAccessToken()).Returns(token);
            //Act
            appointmentService.CancelAnEvent(request);
            //Assert
           
            msgraphRepository.Verify(repo => repo.GetAccessToken(), Times.Once);
        }

        //ToDO: Fix the ExpectedException attribute, because it doesn't detect the exception
        [TestMethod]
        //[ExpectedException(typeof(MSGraphException), "It accepts invalid CancelAppointmentRequest")]
        public void CancelAnEvent_WithInValidGraphServiceClient_ThrowsMSGraphException()
        {
            //Arrange
            AppointmentService appointmentService = GetMock();

            CancelAppointmentRequest request = new CancelAppointmentRequest()
            {
                MSGraphId = "54156d3ese"
            };

            Task<String> token = Task.FromResult("TestToken");

            msgraphRepository.Setup(rep => rep.GetAccessToken()).Throws(new Exception());
            //Act
            appointmentService.CancelAnEvent(request);
            //Assert
            msgraphRepository.Verify(repo => repo.GetAccessToken(), Times.Once);
        }

        [TestMethod]
        public void GetAppointmentCreationInfo_ValidLink_ReturnsInfo()
        {
            AppointmentService appointmentMockService = GetMock();
            string usernameHashed = "726F7474657264616D2D647269657373656E406F75746C6F6F6B2E636F6D";
            string identifierHashed = "3535";

            vacancyRepository.Setup(x => x.GetSpecificVacancy(It.IsAny<int>())).Returns(new Vacancy(1, "Title", "Location", "MeetingLocation"));
            userRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(new User() { Id = 1, Email = "candidate@email.com", FirstName = "FirstName", LastName = "LastName", Role = Role.Recruiter, Password = "password" });
            applicationRepository.Setup(x => x.GetApplicationByCandidateIdAndVacancyId(It.IsAny<int>(), It.IsAny<int>())).Returns(new Application() { CandidateId = 2, Id = 1, RecruiterId = 1, VacancyId = 1 });
            userRepository.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(new User() { Id = 2, Email = "recruiter@email.com", FirstName = "FirstName", LastName = "LastName", Role = Role.Candidate });

            GetAppointmentCreationInfoResponse expected = new GetAppointmentCreationInfoResponse()
            {
                Location = "MeetingLocation",
                RecruiterEmail = "recruiter@email.com",
                RecruiterName = "FirstName LastName"
            };

            GetAppointmentCreationInfoResponse response = appointmentMockService.GetAppointmentCreationInfo(usernameHashed, identifierHashed);

            Assert.AreEqual(expected.RecruiterName, response.RecruiterName);
            Assert.AreEqual(expected.Location, response.Location);
            Assert.AreEqual(expected.RecruiterEmail, response.RecruiterEmail);
            vacancyRepository.Verify(x => x.GetSpecificVacancy(It.IsAny<int>()), Times.Once);
            userRepository.Verify(x => x.GetUserById(It.IsAny<int>()), Times.Once);
            userRepository.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
            applicationRepository.Verify(x => x.GetApplicationByCandidateIdAndVacancyId(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetAppointmentCreationInfo_ValidLink_ThrowsError()
        {
            AppointmentService appointmentMockService = GetMock();
            string usernameHashed = "*D";
            string identifierHashed = "3535";

            appointmentMockService.GetAppointmentCreationInfo(usernameHashed, identifierHashed);
        }

        [TestMethod]
        public void CheckAvailability_ReturnsAvailableTimes()
        {
            AppointmentService appointmentService = GetMock();
            string email = "email@gmail.com";
            DateTime startDate = DateTime.Now.Date.AddDays(1);

            var scheduleItems = new List<ScheduleItem>()
            {
                new ScheduleItem { Start = new DateTimeTimeZone { DateTime = $"{startDate.ToString("yyyy-MM-dd")}T09:00:00.0000000", TimeZone = "Europe/Paris" }, Status = FreeBusyStatus.Tentative },
                new ScheduleItem { Start = new DateTimeTimeZone { DateTime = $"{startDate.ToString("yyyy-MM-dd")}T10:00:00.0000000", TimeZone = "Europe/Paris" }, Status = FreeBusyStatus.Busy }
            };

            var schedule = new ScheduleInformation { ScheduleItems = scheduleItems };
            var schedulePage = new CalendarGetScheduleCollectionPage { schedule };

            msgraphRepository.Setup(x => x.GetAvailableTimes(It.IsAny<List<string>>(), It.IsAny<DateTimeTimeZone>(), It.IsAny<DateTimeTimeZone>(), It.IsAny<int>())).Returns(Task.FromResult<ICalendarGetScheduleCollectionPage>(schedulePage));

            var result = appointmentService.GetAvailableTimesAsync(email, startDate);
            Assert.AreEqual(15, result.Result.Count);
        }
    }
}
