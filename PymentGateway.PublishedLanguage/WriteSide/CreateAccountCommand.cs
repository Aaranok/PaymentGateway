using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class CreateAccountCommand
    {
        public string Name { get; set; }
        public string Cnp { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public int Limit { get; set; }

    }
}
