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
        public string personName { get; set; }
        //public string ServiceName { get; set; }
        public DateTime DateOfTransaction { get; set; }
        //public int ServiceId { get; set; }
        public List<ServiceList> Product = new List<ServiceList>();

    }
}
