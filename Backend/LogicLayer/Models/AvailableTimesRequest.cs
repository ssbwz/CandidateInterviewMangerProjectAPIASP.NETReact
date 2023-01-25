using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
    public class AvailableTimesRequest
    {
        public string RecruiterEmail { get; set; }
        public DateTime Date { get; set; }
    }
}
