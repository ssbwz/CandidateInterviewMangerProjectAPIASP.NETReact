using LogicLayer.Exceptions;
using LogicLayer.Models;
using LogicLayer.Models.Enums;
using LogicLayer.Repositories;
using LogicLayer.Services.Interfaces;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using User = LogicLayer.Models.User;

namespace LogicLayer.Services.Classes
{
    public partial class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository appointmentRepsoitory { get; }
        private IUserRepository userRepository { get; }
        private IApplicationRepository applicationRepository { get; }

        private IApplicationService applicationService { get; }
        private IUserService userService { get; }
        private IVacancyService vacancyService { get; }
        private IMSGraphRepository mSGraphRepository { get; }

        private static readonly HttpClient client = new HttpClient();

        public AppointmentService(IAppointmentRepository appointmentRepsoitory, IUserRepository userRepository
            , IApplicationRepository applicationRepository, IVacancyRepository vacancyRepository
            , IMSGraphRepository mSGraphRepository)
        {
            this.appointmentRepsoitory = appointmentRepsoitory;
            this.userRepository = userRepository;
            this.applicationRepository = applicationRepository;
            this.mSGraphRepository = mSGraphRepository;

            this.applicationService = new ApplicationService(applicationRepository);
            this.userService = new UserService(userRepository);
            this.vacancyService = new VacancyService(vacancyRepository);
        }

        public CreateAppointmentResponse GenerateCreateAppointmentLink(CreateAppointmentLinkRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InsertCandidateInfoRequest.email))
            {
                throw new ArgumentException("Invalid email");
            }
            else if (request.InsertVacancyInfoRequest.id == 0)
            {
                throw new ArgumentException("Invalid vacancy id");
            }

            byte[] usernameHashed = Encoding.ASCII.GetBytes(request.InsertCandidateInfoRequest.email);
            byte[] identifierHashed = Encoding.ASCII.GetBytes(request.InsertVacancyInfoRequest.id.ToString());

            AppointmentLink newAppointmentLink = new AppointmentLink(byteArrayToString(usernameHashed), byteArrayToString(identifierHashed), LinkStatus.Created);

            //Adding the entities to the database         
            InsertUser(request.InsertCandidateInfoRequest);
            InsertUser(request.InsertRecruiterInfoRequest);
            int vacancy_id = InsertVacancy(request.InsertVacancyInfoRequest);
            Models.User? candidate = userService.GetUserByEmail(request.InsertCandidateInfoRequest.email);
            Models.User? recruiter = userService.GetUserByEmail(request.InsertRecruiterInfoRequest.email);
            int candidate_id = candidate == null ? throw new InvalidUserException() : candidate.Id;
            applicationService.InsertApplication(new Models.Application() { VacancyId = vacancy_id, CandidateId = candidate_id, RecruiterId = recruiter.Id });
            appointmentRepsoitory.SaveLinkHashes(newAppointmentLink);

            CreateAppointmentResponse createAppointmentResponse = new CreateAppointmentResponse(newAppointmentLink.Link, newAppointmentLink.LinkStatus.ToString());

            return createAppointmentResponse;
        }

        private string byteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        private String StringToByteArray(string outputA)
        {
            try
            {
                Byte[] bytes = new Byte[outputA.Length / 2];
                for (Int32 i = 0; i < outputA.Length / 2; i++)
                {
                    bytes[i] = Convert.ToByte(outputA.Substring(2 * i, 2), 16);
                }
                return Encoding.ASCII.GetString(bytes);
            }
            catch (Exception)
            {
                throw new ArgumentException($"This value: {outputA} is invalid");
            }
        }

        private void InsertUser(CreateUserInfoRequest request)
        {
            Models.User newUser = null;

            if (request is CreateRecruiterInfoRequest createRecruiterInfoRequest)
            {
                newUser = newUser = new Models.User()
                {
                    FirstName = createRecruiterInfoRequest.first_name,
                    LastName = createRecruiterInfoRequest.last_name,
                    Email = createRecruiterInfoRequest.email,
                    Password = createRecruiterInfoRequest.password,
                    Role = Role.Recruiter
                };
            }
            else
            {

                newUser = newUser = new Models.User()
                {
                    FirstName = request.first_name,
                    LastName = request.last_name,
                    Email = request.email,
                    Role = Role.Candidate
                };
            }

            userService.InsertUser(newUser);
        }

        private int InsertVacancy(CreateVacancyInfoRequest insertVacancyInfoRequest)
        {
            if (vacancyService.ExistsById(insertVacancyInfoRequest.id))
            {
                return insertVacancyInfoRequest.id;
            }

            Models.Vacancy newVacancy = new Models.Vacancy()
            {
                Id = insertVacancyInfoRequest.id,
                Title = insertVacancyInfoRequest.title,
                Location = insertVacancyInfoRequest.location,
                MeetingLocation = insertVacancyInfoRequest.meeting_location
            };

            vacancyService.InsertVacancy(newVacancy);
            return insertVacancyInfoRequest.id;
        }

        public IsAppointmentHashVaildResponse IsAppointmentHashVaild(LinkValidation linkValidation)
        {
            AppointmentLink? appointmentLink = appointmentRepsoitory.GetAppointmentLink(linkValidation);

            if (appointmentLink == null)
            {
                return new IsAppointmentHashVaildResponse("Doesn't exist");
            }

            return new IsAppointmentHashVaildResponse(appointmentLink.LinkStatus.ToString());
        }

        public GetAppointmentCreationInfoResponse GetAppointmentCreationInfo(string usernameHashed, string identifierHashed)
        {
            try
            {
                string email = StringToByteArray(usernameHashed);
                string identifier = StringToByteArray(identifierHashed);
                Models.Vacancy vacancy = vacancyService.GetVacancyById(int.Parse(identifier));
                Models.User user = userService.GetUserByEmail(email);
                Models.Application application = applicationService.GetApplication(user.Id, vacancy.Id);
                Models.User recruiter = userService.GetUserById(application.RecruiterId);
                return new GetAppointmentCreationInfoResponse()
                {
                    Location = vacancy.MeetingLocation,
                    RecruiterEmail = recruiter.Email,
                    RecruiterName = recruiter.FirstName + " " + recruiter.LastName
                };
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<AvailableTime>> GetAvailableTimesAsync(string email, DateTime date)
        {
            date = date.ToLocalTime();
            List<AvailableTime> times = new List<AvailableTime>();
            times.Add(new AvailableTime() { Label = "8:30", Value = "08:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "9:00", Value = "09:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "9:30", Value = "09:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "10:00", Value = "10:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "10:30", Value = "10:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "11:00", Value = "11:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "11:30", Value = "11:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "12:00", Value = "12:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "12:30", Value = "12:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "13:00", Value = "13:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "13:30", Value = "13:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "14:00", Value = "14:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "14:30", Value = "14:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "15:00", Value = "15:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "15:30", Value = "15:30:00.0000000" });
            times.Add(new AvailableTime() { Label = "16:00", Value = "16:00:00.0000000" });
            times.Add(new AvailableTime() { Label = "16:30", Value = "16:30:00.0000000" });
            foreach (AvailableTime time in times.ToList())
            {
                DateTime dt = date;
                double hours = Convert.ToDouble(time.Value.Split(":")[0]);
                dt = dt.AddHours(hours);
                double minutes = Convert.ToDouble(time.Value.Split(":")[1]);
                dt = dt.AddMinutes(minutes);
                if (DateTime.Now > dt)
                {
                    times.Remove(time);
                }
            }

            times = await CheckAvailability(email, date, times);

            return times;
        }

        private async Task<List<AvailableTime>> CheckAvailability(string email, DateTime date, List<AvailableTime> times)
        {
            var schedules = new List<String>()
            {
                email
            };

            var startTime = new DateTimeTimeZone
            {
                DateTime = $"{date.ToString("yyyy-MM-dd")}T08:30:00.0000000",
                TimeZone = "Europe/Paris"
            };

            var endTime = new DateTimeTimeZone
            {
                DateTime = $"{date.ToString("yyyy-MM-dd")}T17:00:00.0000000",
                TimeZone = "Europe/Paris"
            };

            var availabilityViewInterval = 30;

            ICalendarGetScheduleCollectionPage test = await mSGraphRepository.GetAvailableTimes(schedules, startTime, endTime, availabilityViewInterval);

            try
            {
                CalendarGetScheduleCollectionPage item = (CalendarGetScheduleCollectionPage)test;
                List<ScheduleInformation> schedule = (List<ScheduleInformation>)item.CurrentPage;
                foreach (ScheduleInformation s in schedule)
                {
                    List<ScheduleItem> scheduleItems = (List<ScheduleItem>)s.ScheduleItems;
                    foreach (ScheduleItem scheduleItem in scheduleItems)
                    {
                        if (scheduleItem.Status != FreeBusyStatus.Free)
                        {
                            string time = scheduleItem.Start.DateTime.Split("T")[1];
                            try
                            {
                                times.RemoveAt(times.FindIndex(p => p.Value == time));
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                throw new MSGraphException(ex, "Invalid email");
            }

            return times;
        }

        public async Task<List<User>> GetAvailableRecruiters(DateTime startDate)
        {
            startDate = startDate.ToLocalTime();
            List<AvailableTime> times = new List<AvailableTime>();
            times.Add(new AvailableTime() { Label = startDate.ToShortTimeString(), Value = startDate.ToShortTimeString() + ":00.0000000" });

            List<User> allRecruiters = userService.rec();
            List<User> availableRecruiters = new List<User>();
            foreach (User user in allRecruiters)
            {
                List<AvailableTime> availabletimes = await CheckAvailability(user.Email, startDate.Date, times);
                if (availabletimes.Count > 0)
                {
                    availableRecruiters.Add(user);
                }
                else
                {
                    times.Add(new AvailableTime() { Label = startDate.ToShortTimeString(), Value = startDate.ToShortTimeString() + ":00.0000000" });
                }

            }
            return availableRecruiters;

        }

        public CreateAppoinementResponse CreateAppointment(CreateAppointmentRequest request)
        {
            Event createdEvent = insertAppointmentInCalander(request).Result;
            appointmentRepsoitory.UpdateAppointmentLink(new AppointmentLink()
            {
                Id = appointmentRepsoitory.GetAppointmentLink(new LinkValidation(request.CandidateEmailHashed, request.VacancyIdHashed)).Id,
                LinkStatus = LinkStatus.Used
            });

            Appointment newAppointment = new Appointment()
            {
                MSGraphId = createdEvent.Id,
                RecruiterId = userService.GetUserByEmail(request.RecruiterEmail).Id,
                ApplicationId = applicationService.GetApplication(userService.GetUserByEmail(StringToByteArray(request.CandidateEmailHashed)).Id, Convert.ToInt32(StringToByteArray(request.VacancyIdHashed))).Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Location = request.Location

            };

            return new CreateAppoinementResponse()
            {
                appointmentId = appointmentRepsoitory.InsertAppointment(newAppointment)
            };

            //ToDO: Add post method to Driessen to send the meeting info to the client 
        }

        private async Task<Event> insertAppointmentInCalander(CreateAppointmentRequest request)
        {

            string s = StringToByteArray(request.CandidateEmailHashed);

            Models.User candidate = userService.GetUserByEmail(StringToByteArray(request.CandidateEmailHashed));
            Models.User recruiter = userService.GetUserByEmail(request.RecruiterEmail);

            try
            {
                bool isOnline = request.Location == "Online";
                OnlineMeeting onlineMeeting = null;

                var @event = new Event
                {
                    Subject = $"Introduction meeting with {candidate.FullName}",
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = $"<div><Strong>Candidate scheduled a meeting</Strong>" +
                        $"<br> <br>" +
                        $"<a href = \'https://mijn.driessen.nl/Profiles/Profile/Details?candidateId=%candidate{candidate.Id}\' > Candidate profile </a>" +
                        $"<br> <p> Created by the candidate at {DateTime.Now.ToString("yyyy / MM / dd hh:mm tt")}</p> </div>"

                    },
                    Start = new DateTimeTimeZone
                    {
                        DateTime = request.StartDate.ToString("yyyy-MM-dd") + "T" + request.StartDate.ToString("HH:mm:ss"),
                        TimeZone = "UTC"
                    },
                    End = new DateTimeTimeZone
                    {
                        DateTime = request.EndDate.ToString("yyyy-MM-dd") + "T" + request.EndDate.ToString("HH:mm:ss"),
                        TimeZone = "UTC"
                    },
                    Attendees = new List<Attendee>()
    {

        new Attendee
        {
            EmailAddress = new EmailAddress
            {
                Address = recruiter.Email,
                Name = recruiter.FullName,
            },
            Type = AttendeeType.Required
        }
    },
                    AllowNewTimeProposals = false,
                    IsOnlineMeeting = isOnline,
                    OnlineMeetingProvider = OnlineMeetingProviderType.TeamsForBusiness
                };

                if (!isOnline)
                {
                    @event.Location = new Location
                    {
                        DisplayName = request.Location,

                    };
                }

                Event createdEvent = await mSGraphRepository.CreateEvent(@event);

                string joinUrl = null;
                if (createdEvent.OnlineMeeting != null)
                {
                    joinUrl = createdEvent.OnlineMeeting.JoinUrl;
                }
                sendCandidateInvitation(request, candidate, joinUrl);

                return createdEvent;
            }
            catch (Exception ex)
            {
                throw new MSGraphException(ex, "Faild to schedule a event");
            }


            async void sendCandidateInvitation(CreateAppointmentRequest request, Models.User candidate, string teamsLink)
            {
                try
                {
                    string locationMessage = null;
                    if (teamsLink != null)
                    {
                        locationMessage = $"<a href = {teamsLink} ><Strong>Meeting link</Strong></a>";
                    }
                    else
                        locationMessage = $"<p> At {request.Location} </p>";

                    var message = new Message
                    {
                        Subject = "Driessen first interview",
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Html,
                            Content = $"Hello {candidate.FirstName} {candidate.LastName}" +
                            $"<p>You scheduled a meeting with our recruiter successfully on {request.StartDate.ToString("yyyy-MM-dd")} at {request.StartDate.ToLocalTime().ToString("HH:mm")}</p>" +
                            $"<br> <br>" +
                            $"{locationMessage}" +
                            $""
                        },
                        ToRecipients = new List<Recipient>()
                        {
                            new Recipient
                            {
                                EmailAddress = new EmailAddress
                                {
                                    Address = candidate.Email
                                }
                            }
                        }
                    };

                    var saveToSentItems = false;

                    mSGraphRepository.sendCandidateInvitation(message);

                }
                catch (Exception ex)
                {
                    throw new MSGraphException(ex, "Faild to send an invitation");
                }

            }
        }

        public async void SendEmail(Email email)
        {
            try
            {
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

                var saveToSentItems = false;
                
                mSGraphRepository.SendEmail(message, saveToSentItems);
            }
            catch (Exception ex)
            {
                throw new MSGraphException(ex, "Failed to send an email");
            }
        }


        public async void CancelAnEvent(CancelAppointmentRequest request)
        {
            try
            {
                //https://graph.microsoft.com/v1.0/users/3d77407c-0a5d-4da9-997c-39127bd90f2e/events/" + request.MSGraphId + "/cancel"

                var client = new RestClient("https://graph.microsoft.com/v1.0");
                var req = new RestRequest("https://graph.microsoft.com/v1.0/users/3d77407c-0a5d-4da9-997c-39127bd90f2e/events/" + request.MSGraphId + "/cancel");
                req.AddHeader("Authorization", "Bearer " + mSGraphRepository.GetAccessToken().Result);

                req.RequestFormat = DataFormat.Json;
                req.AddJsonBody(new { Comment = "" });
                client.Post(req);
            }
            catch (Exception ex)
            {
                throw new MSGraphException(ex, "Faild to cancel the event");
            }

        }


        /// <summary>
        /// An example of how to authenticate the Microsoft Graph SDK using the MSAL library
        /// </summary>
        /// <returns></returns>
        private GraphServiceClient GetAuthenticatedGraphClient()
        {
            AuthenticationConfig config = new AuthenticationConfig();

            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = IsAppUsingClientSecret(config);

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

            if (isUsingClientSecret)
            {
                // Even if this is a console application here, a daemon application is a confidential client application
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            else
            {
                ICertificateLoader certificateLoader = new DefaultCertificateLoader();
                certificateLoader.LoadIfNeeded(config.Certificate);

                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithCertificate(config.Certificate.Certificate)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            app.AddInMemoryTokenCache();


            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator. 
            string[] scopes = new string[] { $"{config.ApiUrl}.default" }; // Generates a scope -> "https://graph.microsoft.com/.default"

            GraphServiceClient graphServiceClient =
                    new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                        AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                        // Add the access token in the Authorization header of the API request.
                        requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));

            return graphServiceClient;
        }


        private async Task<AuthenticationResult> GetAccessToken()
        {
            AuthenticationConfig config = new AuthenticationConfig();

            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = IsAppUsingClientSecret(config);

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

            if (isUsingClientSecret)
            {
                // Even if this is a console application here, a daemon application is a confidential client application
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            else
            {
                ICertificateLoader certificateLoader = new DefaultCertificateLoader();
                certificateLoader.LoadIfNeeded(config.Certificate);

                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithCertificate(config.Certificate.Certificate)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            app.AddInMemoryTokenCache();


            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator. 
            string[] scopes = new string[] { $"{config.ApiUrl}.default" }; // Generates a scope -> "https://graph.microsoft.com/.default"

            GraphServiceClient graphServiceClient =
                    new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                        AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                        // Add the access token in the Authorization header of the API request.
                        requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));
     
            return  app.AcquireTokenForClient(scopes)
                   .ExecuteAsync().Result;
        }


        /// <summary>
        /// Checks if the sample is configured for using ClientSecret or Certificate. This method is just for the sake of this sample.
        /// You won't need this verification in your production application since you will be authenticating in AAD using one mechanism only.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <returns></returns>
        private bool IsAppUsingClientSecret(AuthenticationConfig config)
        {
            string clientSecretPlaceholderValue = "[Enter here a client secret for your application]";

            if (!String.IsNullOrWhiteSpace(config.ClientSecret) && config.ClientSecret != clientSecretPlaceholderValue)
            {
                return true;
            }

            else if (config.Certificate != null)
            {
                return false;
            }

            else
                throw new Exception("You must choose between using client secret or certificate. Please update appsettings.json file.");
        }
        
        public GetSpecificAppointmentResponse GetSpecificAppointment(int appointment)
        {
            GetSpecificAppointmentResponse response = new GetSpecificAppointmentResponse();
            Models.Appointment appointmentDetails = appointmentRepsoitory.GetSpecificAppointment(Convert.ToInt32(appointment));
            Models.User recruiterDetails = userRepository.GetRecruiterUserByApplicationIdFromAppointment(appointmentDetails.ApplicationId);
            Models.Application application = applicationRepository.GetSpecificApplication(appointmentDetails.ApplicationId);
            response.Id = appointmentDetails.Id;
            response.RecruiterFirstName = recruiterDetails.FirstName;
            response.RecruiterLastName = recruiterDetails.LastName;
            response.CandidateFirstName = application.Candidate.FirstName;
            response.CandidateLastName = application.Candidate.LastName;
            response.StartDate = appointmentDetails.StartDate;
            response.EndDate = appointmentDetails.EndDate;
            response.Location = appointmentDetails.Location;
            response.MSGraphId = appointmentDetails.MSGraphId;
            return response;
        }

        public GetSpecificAppointmentResponse ChangeRecruiter(string email, int appointmentID)
        {
            GetSpecificAppointmentResponse response = new GetSpecificAppointmentResponse();
            Models.Appointment appointmentDetails = appointmentRepsoitory.GetSpecificAppointment(appointmentID);
            Models.User substitudeRecruiter = userRepository.GetSpecificRecruiterByEmail(email);
            Models.Application application = applicationRepository.GetSpecificApplication(appointmentDetails.ApplicationId);

            appointmentDetails.RecruiterId = substitudeRecruiter.Id;


            CancelAppointmentRequest request = new CancelAppointmentRequest();
            request.MSGraphId = appointmentDetails.MSGraphId;
            CancelAnEvent(request);

            CreateAppointmentRequest createAppointmentRequest = new CreateAppointmentRequest();
            createAppointmentRequest.StartDate = appointmentDetails.StartDate;
            createAppointmentRequest.EndDate = appointmentDetails.EndDate;
            createAppointmentRequest.Location = appointmentDetails.Location;
            createAppointmentRequest.CandidateEmailHashed = byteArrayToString(Encoding.ASCII.GetBytes(application.Candidate.Email));
            createAppointmentRequest.RecruiterEmail = email;
            createAppointmentRequest.VacancyIdHashed = byteArrayToString(Encoding.ASCII.GetBytes(application.VacancyId.ToString()));
            Event createdEvent = insertAppointmentInCalander(createAppointmentRequest).Result;

            appointmentDetails.MSGraphId = createdEvent.Id;

            appointmentDetails = appointmentRepsoitory.UpdateSpecificAppointment(appointmentDetails);

            response.Id = appointmentDetails.Id;
            // if(appointmentDetails.RecruiterId == substitudeRecruiter.Id)
            //{ 
            response.RecruiterFirstName = substitudeRecruiter.FirstName;
            response.RecruiterLastName = substitudeRecruiter.LastName;
            //}
            response.CandidateFirstName = application.Candidate.FirstName;
            response.CandidateLastName = application.Candidate.LastName;
            response.StartDate = appointmentDetails.StartDate;
            response.EndDate = appointmentDetails.EndDate;
            response.Subject = appointmentDetails.Subject;
            response.Location = appointmentDetails.Location;
            response.MSGraphId = appointmentDetails.MSGraphId;
            return response;
        }

        public bool DeleteSpecificAppointment(int appointmentID)
        {
            Models.Appointment appointmentDetails = appointmentRepsoitory.GetSpecificAppointment(appointmentID);

            CancelAppointmentRequest request = new CancelAppointmentRequest();
            request.MSGraphId = appointmentDetails.MSGraphId;
            CancelAnEvent(request);

            bool result = appointmentRepsoitory.DeleteSpecificAppointment(appointmentDetails);
            return result;
        }

        public GetAppointmentsResponse GetAllAppointments()
        {
            return this.appointmentRepsoitory.getAppointments();
        }

        public GetAppointmentsResponse GetAppointmentsByRecruiterId(int id)
        {
            return this.appointmentRepsoitory.getAppointmentsByRecruiterId(id);
        }

        public GetAppointmentsResponse GetAllAppointmentsByDateAscending()
        {
            return this.appointmentRepsoitory.getAppointmentsFilterByDateAscending();
        }

        public GetAppointmentsResponse GetAllAppointmentsByDateDecending()
        {
            return this.appointmentRepsoitory.getAppointmentsFilterByDateDecending();
        }

        public GetAppointmentsResponse GetAppointmentsByRecruiterName(string name)
        {
            return this.appointmentRepsoitory.GetAppointmentsByRecruiterName(name);
        }



    }
}
