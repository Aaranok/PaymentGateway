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
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public decimal Limit { get; set; }
        public int Id { get; set; }

    }
}
