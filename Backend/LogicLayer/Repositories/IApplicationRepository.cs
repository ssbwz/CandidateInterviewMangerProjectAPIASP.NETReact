using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Repositories
{
    public interface IApplicationRepository
    {
        Application GetSpecificApplication(int application);
        List<Application> GetApplicationssByVacancy(int vacancy);
        void InsertApplication(Application application);
        Application GetApplicationByCandidateIdAndVacancyId(int candidateId, int vacancyId);

    }
}
