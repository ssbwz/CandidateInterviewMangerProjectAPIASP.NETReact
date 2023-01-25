using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
    public class GetAppointmentCreationInfoResponse
    {
        public string RecruiterEmail { get; set; }
        public string RecruiterName { get; set; }
        public string Location { get; set; }
    }

    public class AvailableTime
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
