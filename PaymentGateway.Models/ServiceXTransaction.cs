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
