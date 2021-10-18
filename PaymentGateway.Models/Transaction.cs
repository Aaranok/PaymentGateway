using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public DateTime DateOfOperation { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public int Id { get; set; } 
        public decimal Value { get; set; }
        public DateTime GetOpDate()
        {
            return this.DateOfTransaction.AddDays(2);
        }
        public Transaction()
        {

        }
    }
}
