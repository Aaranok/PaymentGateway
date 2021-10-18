using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ServiceCreated: INotification
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int Limit { get; set; }
        public string Currency { get; set; }

        public ServiceCreated(string name, decimal value, int limit, string currency)
        {
            this.Currency = currency;
            this.Limit = limit;
            this.Name = name;
            this.Value = value;
        }
    }
}
