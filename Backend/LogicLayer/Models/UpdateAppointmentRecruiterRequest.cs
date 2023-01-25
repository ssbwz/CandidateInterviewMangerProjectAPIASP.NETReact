using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
   public class UpdateAppointmentRecruiterRequest
    {
        public int Id { get; set; }
        public string RecruiterEmail { get; set; }
    }
}
