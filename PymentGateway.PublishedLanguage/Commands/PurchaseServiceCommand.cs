using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentGateway.Models.ServiceXTransaction;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class PurchaseServiceCommand: IRequest
    {
        public string Iban { get; set; }
        public string Cnp { get; set; }
        public string PersonName { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public List<ServiceList> Product = new();

    }
}
