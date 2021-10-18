using System;

namespace PaymentGateway.Models
{
    public class Account
    {
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string IbanCode { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal Limit { get; set; }
        public int AccountID { get; set; }
        public int PersonID { get; set; }

        public Account(decimal balance, string currency, string IbanCode, string type, string status, decimal limit, int id, int persId)
        {
            this.Balance = balance;
            this.Currency = currency;
            this.IbanCode = IbanCode;
            this.Limit = limit;
            this.Status = status;
            this.Type = type;
            this.AccountID = id;
            this.PersonID = persId;
        }
        public Account()
        {

        }

    }
}
