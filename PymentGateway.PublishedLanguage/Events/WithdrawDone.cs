using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class WithdrawDone: INotification
    {
        public decimal Amount;
        public string Iban;
        public string Currency;
        public DateTime DateOfOperation;
        public WithdrawDone(string iban, string currency, decimal value, DateTime date)
        {
            this.Currency = currency;
            this.Iban = iban;
            this.Amount = value;
            this.DateOfOperation = date;
        }

    }
}
