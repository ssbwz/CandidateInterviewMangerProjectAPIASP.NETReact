

namespace LogicLayer.Models
{
    public class CreateAppointmentLinkRequest
    {
        public CreateVacancyInfoRequest InsertVacancyInfoRequest { get; set; }
        public CreateUserInfoRequest InsertCandidateInfoRequest { get; set; }
        public CreateRecruiterInfoRequest InsertRecruiterInfoRequest { get; set; }

    }

    public class CreateVacancyInfoRequest
    {
        public int id { get; set; }
        public string title { get; set; } 
        public string location { get; set; }
        public string meeting_location { get; set; }
    }
    public class CreateUserInfoRequest
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }

    public class CreateRecruiterInfoRequest : CreateUserInfoRequest
    {
        public string password { get; set; }
    }

}
