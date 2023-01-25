using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Models
{
    public class LinkValidation
    {

        public string UsernameHashed { get; }
        public string IdentifierHashed { get; }

        public LinkValidation(string usernameHashed,string identifierHashed) {
            UsernameHashed = usernameHashed;
            IdentifierHashed = identifierHashed;
        }
    }
}
