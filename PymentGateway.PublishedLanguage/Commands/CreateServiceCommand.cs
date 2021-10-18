using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class CreateServiceCommand:IRequest
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int Limit { get; set; }
        public string Currency { get; set; }

    }
}
