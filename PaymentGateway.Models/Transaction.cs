﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentGateway.Models
{
    public partial class Transaction
    {
        public Transaction()
        {
            ProductXtransactions = new HashSet<ProductXtransaction>();
        }

        public int Id { get; set; }
        public int? AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public DateTime DateOfOperation { get; set; }
        public DateTime DateOfTransaction { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<ProductXtransaction> ProductXtransactions { get; set; }
    }
}
