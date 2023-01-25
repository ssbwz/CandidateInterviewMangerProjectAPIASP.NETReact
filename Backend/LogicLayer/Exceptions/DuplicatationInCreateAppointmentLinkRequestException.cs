using System.Net;
using System.Web.Http;

namespace LogicLayer.Exceptions
{
    [Serializable]
    public class DuplicatationInCreateAppointmentLinkRequestException : HttpResponseException
    {

        public DuplicatationInCreateAppointmentLinkRequestException(HttpStatusCode statusCode, string message) : base(statusCode)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}
