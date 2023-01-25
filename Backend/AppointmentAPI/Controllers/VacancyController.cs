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
    [Route("/vacancies")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private IVacancyService vacancyService { get; }
        public IDecoderService DecoderService { get; set; }

        public VacancyController(IDecoderService decoderService)
        {
            IVacancyRepository vacancyRepository = new VacancyRepository();
            this.vacancyService = new VacancyService(vacancyRepository);
            DecoderService = decoderService;
        }

        [HttpGet]
        public ActionResult<List<Vacancy>> GetAllVacancies()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return vacancyService.GetAllVacancies();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }

        [HttpGet]
        [Route("/vacancy")]
        public ActionResult<Vacancy> GetSpecificVacancy(int vacancyId)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });
                return vacancyService.GetVacancyById(vacancyId);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }
    }
}
