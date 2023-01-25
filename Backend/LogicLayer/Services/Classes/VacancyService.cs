
using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services.Interfaces;

namespace LogicLayer.Services.Classes
{
    public class VacancyService : IVacancyService
    {
        private IVacancyRepository vacancyRepository;

        public VacancyService(IVacancyRepository vacancyRepository)
        {
            this.vacancyRepository = vacancyRepository;
        }

        public bool ExistsById(int id)
        {
            return vacancyRepository.ExistsById(id);
        }
        
        public int InsertVacancy(Models.Vacancy vacancy) {
            if (ExistsById(vacancy.Id))
            {
                return vacancy.Id;
            }

            vacancyRepository.InsertVacancy(vacancy);
            return vacancy.Id;
           
        }

        public Models.Vacancy GetVacancyById(int vacancyid) => vacancyRepository.GetSpecificVacancy(vacancyid);

        public List<Vacancy> GetAllVacancies()
        {
            return vacancyRepository.GetAllVacancies();
        }
    }
}
