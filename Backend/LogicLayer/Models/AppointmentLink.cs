using LogicLayer.Models.Enums;

namespace LogicLayer.Models
{
    public class AppointmentLink
    {
        //ToDo: When deploying the website change the hostAddress to the server address
        private readonly string hostAddress = "http://localhost:3000/";

        public AppointmentLink()
        {
        }

        public AppointmentLink(string usernameHashed,string identifierHashed, LinkStatus linkStatus)
        {
            this.EmailHashed = usernameHashed;
            this.IdentifierHashed = identifierHashed;
            this.LinkStatus = linkStatus;
        }

        public AppointmentLink(int id,string usernameHashed, string identifierHashed, LinkStatus linkStatus)
            : this(usernameHashed,identifierHashed,linkStatus)
        {
            this.Id = id;
        }

        public int Id { get;  set; }

        public string Link { 
            get {
                return hostAddress + "appointment/create/" + EmailHashed + "/" + IdentifierHashed;
            }
        }
        public LinkStatus LinkStatus { get; set; }
        public string EmailHashed { get; private set; }
        public string IdentifierHashed { get; private set; }

        
    }
}
