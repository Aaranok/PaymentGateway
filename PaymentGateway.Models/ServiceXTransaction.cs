using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class ServiceXTransaction
    {
        public struct ServiceList
        {
            public int IdService;
            public int NoPurchased;
        }

        public int IdTransaction;
        public ServiceList ServiceIdList;
        public double Value;
        

    }
}
