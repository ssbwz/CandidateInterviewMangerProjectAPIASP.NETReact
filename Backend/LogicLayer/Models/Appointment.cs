using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string MSGraphId { get; set; }
        public int RecruiterId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Location { get; set; }
    }
}
