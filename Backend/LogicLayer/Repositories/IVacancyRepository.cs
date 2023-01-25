using LogicLayer.Models;

namespace LogicLayer.Repositories
{
    public interface IVacancyRepository
    {
        List<Vacancy> GetAllVacancies();
        Vacancy GetSpecificVacancy(int vacancy);
        bool ExistsById(int id);
        void InsertVacancy(Vacancy vacancy);
    }
}
