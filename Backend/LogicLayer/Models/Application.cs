using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int VacancyId { get; set; }
        public int CandidateId { get; set; }
        public int RecruiterId { get; set; }
        public Vacancy? JobVacancy { get; set; }
        public User Candidate { get; set; }
    }
}
