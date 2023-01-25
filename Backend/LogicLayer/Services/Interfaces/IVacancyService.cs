using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Interfaces
{
    public interface IVacancyService
    {
        bool ExistsById(int id);
        int InsertVacancy(Models.Vacancy vacancy);
        Models.Vacancy GetVacancyById(int vacancyid);
        List<Vacancy> GetAllVacancies();
    }
}
