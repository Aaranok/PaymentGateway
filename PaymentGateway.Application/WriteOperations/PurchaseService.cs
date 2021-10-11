using PaymentGateway.Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{

    public class PurchaseService: IWriteOperations<PurchaseServiceCommand>
    {
        public IEventSender eventSender;

        public PurchaseService(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void PerformOperation(PurchaseServiceCommand operation, Database database)
        {
            var random = new Random();
            var account = database.GetAccountByInfo(operation.Iban);
           // var person = database.GetPersonByCnp(operation.Cnp);

            if (account == null)
            {
                throw new Exception("Account not found");
            }
            double totalValue = 0;
            string currency = "";
            foreach(var item in operation.Product)
            {
                var service = database.GetServiceFromId(item.IdService);
                if (service == null)
                {
                    throw new Exception("Service not found");
                }
                if (service.Limit < item.NoPurchased)
                {
                    throw new Exception("Not enough items in storage");

                }
                totalValue += service.Value;
                currency = service.Currency;
                service.Limit -= item.NoPurchased;
            }
            
            if ( totalValue > account.Balance)
            {
                throw new Exception("Not enough capital");
            }

            Transaction transaction = new Transaction();
            
            transaction.Amount = totalValue;
            transaction.Currency = currency;
            transaction.DateOfTransaction = operation.DateOfTransaction;
            transaction.DateOfOperation = transaction.GetOpDate();
            transaction.Type = "Purchase";
            transaction.Id = database.Transactions.Count() + 1;

            database.Transactions.Add(transaction);

            account.Balance -= totalValue;

            foreach (var item in operation.Product)
            {
                ServiceXTransaction servXTransItem = new ServiceXTransaction();
                servXTransItem.IdTransaction = transaction.Id;
                servXTransItem.ServiceIdList.IdService = item.IdService;
                servXTransItem.ServiceIdList.NoPurchased = item.NoPurchased;
                database.ServXTrans.Add(servXTransItem);
            }

            database.SaveChange();
            ServicePurchased eventServPurchased = new(operation.Iban, operation.Cnp, operation.personName, operation.Product);
            eventSender.SendEvent(eventServPurchased);




        }
    }
}
