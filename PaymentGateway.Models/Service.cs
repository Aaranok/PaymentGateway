using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class Service
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }
        public int Limit { get; set; }

    }
}
