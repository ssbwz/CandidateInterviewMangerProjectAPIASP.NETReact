using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Interfaces
{
    public interface IApplicationService
    {
        Application GetApplication(int candidateId, int vacancyId);
        void InsertApplication(Application application);
        List<Application> GetApplicationsByVacancy(int vacancy);
    }
}
