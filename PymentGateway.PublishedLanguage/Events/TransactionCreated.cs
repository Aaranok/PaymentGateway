using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class TransactionCreated: INotification
    {
        public DateTime DateOfTransaction;
        public string Type;
        public string Currency;
        public double Amount;
        public string Iban;
        public TransactionCreated(DateTime date, string type, string currency, double value, string iban)
        {
            this.DateOfTransaction = date;
            this.Type = type;
            this.Currency = currency;
            this.Amount = value;
            this.Iban = iban;
        }
    }
}
