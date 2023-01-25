using LogicLayer.Models.Enums;

namespace LogicLayer.Models
{
    public class IsAppointmentHashVaildResponse
    {
        public string LinkStatus { get; }

        public IsAppointmentHashVaildResponse(string linkStatus)
        {
            LinkStatus = linkStatus;
        }

    }
}
