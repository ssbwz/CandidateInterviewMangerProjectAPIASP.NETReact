using AppointmentAPI.DecoderService;
using DataAccessLayer.Repositories;
using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services.Classes;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Controllers
{
    [Route("/SingleVacancy")]
    [ApiController]
    public class SingleVacancyController
    {
        private IApplicationService _applicationService { get; }
        public IDecoderService DecoderService { get; set; }

        public SingleVacancyController(IDecoderService decoderService)
        {
            IApplicationRepository applicationRepository = new ApplicationRepository();
            this._applicationService = new ApplicationService(applicationRepository);
            DecoderService = decoderService;
        }

        [HttpGet]
        public ActionResult<List<Application>> GetCandidatesForSingleVacancy(int vacancyId)
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter", "Admin" });


                return this._applicationService.GetApplicationsByVacancy(vacancyId);
            }
            catch (UnauthorizedAccessException uae)
            {
                return null;
            }
        }
    }
}
