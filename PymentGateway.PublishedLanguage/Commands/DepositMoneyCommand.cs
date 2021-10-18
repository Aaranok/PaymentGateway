using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class DepositMoneyCommand: IRequest
    {
        public string Iban { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfOperation { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
