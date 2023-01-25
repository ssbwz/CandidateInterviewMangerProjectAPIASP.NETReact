using Microsoft.Graph;

namespace LogicLayer.Models
{
    public class Email
    {
        public string subject { get; set; }
        public string bodyAsHtml { get; set; }
        public List<Recipient> ToRecipients { get; set; }
    }
}
