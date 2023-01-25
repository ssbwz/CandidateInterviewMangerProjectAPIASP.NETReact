

namespace LogicLayer.Models
{
    public class CreateAppointmentRequest
    {
        public string CandidateEmailHashed { get; set; }
        public string VacancyIdHashed { get; set; }
        public string RecruiterEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
    }
}
