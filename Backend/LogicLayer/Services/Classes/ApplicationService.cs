using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Classes
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public Application GetApplication(int candidateId, int vacancyId)
        {
            return applicationRepository.GetApplicationByCandidateIdAndVacancyId(candidateId, vacancyId);
        }
        public void InsertApplication(Application application)
        {
            applicationRepository.InsertApplication(application);
        }

        public List<Application> GetApplicationsByVacancy(int vacancy) => applicationRepository.GetApplicationssByVacancy(vacancy);
    }
}
