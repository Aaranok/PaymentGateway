using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentGateway.Models.ServiceXTransaction;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ServicePurchased: INotification
    {
        public string Iban { get; set; }
        public string Cnp { get; set; }
        public string PersonName { get; set; }
        public List<ServiceList> ServiceIdList = new List<ServiceList>();


        public ServicePurchased(string iban, string cnp, string personName, List<ServiceList> serviceIdList){
            this.Iban = iban;
            this.Cnp = cnp;
            this.PersonName = personName;
            this.ServiceIdList = serviceIdList;
        }
            
    }
}
