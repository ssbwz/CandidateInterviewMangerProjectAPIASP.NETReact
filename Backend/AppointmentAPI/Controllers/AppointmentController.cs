using DataAccessLayer.Repositories;
using LogicLayer.Models;
using LogicLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using LogicLayer.Services.Interfaces;
using LogicLayer.Services.Classes;
using DataAccessLayer;
using LogicLayer.Exceptions;
using AppointmentAPI.DecoderService;

namespace AppointmentAPI.Controllers
{
    [ApiController]
    [Route("/appointments")]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService appointmentService { get; }

        public IDecoderService DecoderService { get; }


        public AppointmentController(IDecoderService decoderService)
        {
            IAppointmentRepository appointmentRepository = new AppointmentRepository();
            IUserRepository userRepository = new UserRepository();
            IApplicationRepository applicationRepository = new ApplicationRepository();
            IVacancyRepository vacancyRepository = new VacancyRepository();
            IMSGraphRepository msgraphRepository = new MSGraphRepository();
            DecoderService = decoderService;
             this.appointmentService = new AppointmentService(appointmentRepository, userRepository,         applicationRepository, vacancyRepository, msgraphRepository);
        }


        [HttpPost]
        [Route("/createLink")]
        public ActionResult<CreateAppointmentResponse> CreateNewAppointmentLink([System.Web.Http.FromUri] CreateAppointmentLinkRequest request)
        {
            return appointmentService.GenerateCreateAppointmentLink(request);

        }

        [HttpGet]
        [Route("/verifyLink")]
        public ActionResult<IsAppointmentHashVaildResponse> IsAppointmentHashVaild(string usernameHashed, string identifierHashed)
        {
            return appointmentService.IsAppointmentHashVaild(new LinkValidation(usernameHashed, identifierHashed));

        }

        [HttpGet("{id}")]
        public ActionResult<GetSpecificAppointmentResponse> GetAppointment(int id)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return Ok(appointmentService.GetSpecificAppointment(id));
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<GetSpecificAppointmentResponse> UpdateRecruiterOfAnAppointment([System.Web.Http.FromUri] UpdateAppointmentRecruiterRequest updateAppointment)
        {

            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                GetSpecificAppointmentResponse updatedAppointment = appointmentService.ChangeRecruiter(updateAppointment.RecruiterEmail, updateAppointment.Id);
                return Ok(updatedAppointment);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }


        //Maybe it is a good idea to change it from void to list of all appointments
        [HttpDelete("{id}")]
        public ActionResult<HttpResponseMessage> DeleteAppointment(int id)
        {
            DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
            bool result = appointmentService.DeleteSpecificAppointment(id);
            try
            {
                if (result == true)
                {
                    HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    return response;
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                    return response;
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.StackTrace}");
            }
        }

        [HttpGet]
        public ActionResult<GetAppointmentsResponse> getAllAppointments()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return appointmentService.GetAllAppointments();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/FilterDateAscending")]
        public ActionResult<GetAppointmentsResponse> getAllAppointmentsFilterDateAscending()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return appointmentService.GetAllAppointmentsByDateAscending();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/FilterDateDescending")]
        public ActionResult<GetAppointmentsResponse> getAllAppointmentsFilterDateDescending()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return appointmentService.GetAllAppointmentsByDateDecending();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/searchByRecruiterName/{name}")]
        public ActionResult<GetAppointmentsResponse> GetAppointmentsBySearchSubject(string name)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return appointmentService.GetAppointmentsByRecruiterName(name);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/getByRecruiter/{id}")]
        public ActionResult<GetAppointmentsResponse> GetAppointmentsByRecruiterId(int id)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return appointmentService.GetAppointmentsByRecruiterId(id);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpPost]
        public ActionResult<CreateAppoinementResponse> CreateAppointment([System.Web.Http.FromUri] CreateAppointmentRequest request)
        {
            try
            {
                return Created("", appointmentService.CreateAppointment(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DALException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/appointmentCreationInfo")]
        public ActionResult<GetAppointmentCreationInfoResponse> GetAppointmentCreationInfo(string usernameHashed, string identifierHashed)
        {
            try
            {
                return appointmentService.GetAppointmentCreationInfo(usernameHashed, identifierHashed);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DALException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/availableTimes")]
        public async Task<ActionResult<List<AvailableTime>>> GetAvailableTimesForRecruiter([System.Web.Http.FromUri] AvailableTimesRequest request)
        {
            try
            {
                return await appointmentService.GetAvailableTimesAsync(request.RecruiterEmail, request.Date);
            }
            catch (MSGraphException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/available/{date}")]
        public async Task<List<User>> GetAvailableRecruitersForTimeSlot(DateTime date)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return await appointmentService.GetAvailableRecruiters(date);
            }
            catch (UnauthorizedAccessException uae)
            {
                Unauthorized(uae.Message);
            }
            return null;
        }

    }
}