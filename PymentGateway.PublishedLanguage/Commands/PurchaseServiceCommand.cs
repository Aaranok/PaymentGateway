using MediatR;
using System;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class PurchaseServiceCommand: IRequest
    {
        public string Iban { get; set; }
        public string Cnp { get; set; }
        public string PersonName { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
