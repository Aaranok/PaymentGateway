using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class CreateServiceCommand
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public int Limit { get; set; }
        public string Currency { get; set; }

    }
}
