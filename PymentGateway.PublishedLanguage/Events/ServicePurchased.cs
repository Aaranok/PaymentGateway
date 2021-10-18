using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ServicePurchased: INotification
    {
        public string Iban { get; set; }
        public string Cnp { get; set; }
        public string PersonName { get; set; }
        //public List<ServiceList> ServiceIdList = new();


        public ServicePurchased(string iban, string cnp, string personName){
            this.Iban = iban;
            this.Cnp = cnp;
            this.PersonName = personName;
            //this.ServiceIdList = serviceIdList;
        }
            
    }
}
