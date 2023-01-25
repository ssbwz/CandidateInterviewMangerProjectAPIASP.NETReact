using LogicLayer.Models.Enums;

namespace LogicLayer.Models
{
    public class CreateAppointmentResponse
    {
        public CreateAppointmentResponse(string newAppointmentLink,string linkStatus)
        {
            NewAppointmentLink = newAppointmentLink;
            LinkStatus = linkStatus;
        }

        public string NewAppointmentLink { get; }
        public string LinkStatus { get; }
    }
}
