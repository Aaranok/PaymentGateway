using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentGateway.Models
{
    public partial class ProductXtransaction
    {
        public int IdTransaction { get; set; }
        public int IdService { get; set; }
        public int NoPurchased { get; set; }
        public decimal Value { get; set; }

        public virtual Product IdServiceNavigation { get; set; }
        public virtual Transaction IdTransactionNavigation { get; set; }
    }
}
