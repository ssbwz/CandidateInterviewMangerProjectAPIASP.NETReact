using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
    public class GetSpecificAppointmentResponse
    {
        public int Id { get; set; }
        public string RecruiterFirstName { get; set; }
        public string RecruiterLastName { get; set; }
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string MSGraphId { get; set; }
    }
}
