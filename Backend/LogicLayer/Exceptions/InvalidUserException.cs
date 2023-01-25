using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LogicLayer.Exceptions
{

    [Serializable]
    public class InvalidUserException : HttpResponseException
    {

        public InvalidUserException(string message) : base(HttpStatusCode.BadRequest)
        {
            this.Message = message;
        }
        public InvalidUserException() : base(HttpStatusCode.BadRequest)
        {
            this.Message = "INVALID_USER";
        }



        public string Message { get; private set; }
    }
}
