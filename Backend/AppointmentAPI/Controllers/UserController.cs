using AppointmentAPI.DecoderService;
using DataAccessLayer.Repositories;
using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services.Classes;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private IUserService userService { get; }
        public IDecoderService DecoderService { get; set; }

        public UserController(IDecoderService decoderService)
        {
            IUserRepository userRepository = new UserRepository();
            this.userService = new UserService(userRepository);
            DecoderService = decoderService;

        }

        [HttpGet]
        [Route("/recruiter/{id:int}")]
        public ActionResult<User> GetRecruiterUserByApplicationIdFromAppointment(int id)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return userService.GetRecruiterUserByApplicationIdFromAppointment(id);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/candidate/{id:int}")]
        public ActionResult<User> GetCandidateUserByApplicationIdFromAppointment(int id)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return userService.GetCandidateUserByApplicationIdFromAppointment(id);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/candidate")]
        public ActionResult<List<User>> FindCandidatesByRole()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return userService.can();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/recruiter")]
        public ActionResult<List<User>> FindRecruitersByRole()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return userService.rec();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }
    }
}
