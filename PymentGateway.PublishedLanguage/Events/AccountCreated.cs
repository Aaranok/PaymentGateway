using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class AccountCreated:INotification
    {
        public string Iban { get; set; }
        public string Cnp { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public AccountCreated(string iban, string cnp, string name, string type, string status)
        {
            this.Name = name;
            this.Cnp = cnp;
            this.Iban = iban;
            this.Type = type;
            this.Status = status;
        }
    }
}
