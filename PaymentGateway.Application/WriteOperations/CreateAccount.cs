using PaymentGateway.Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using PaymentGateway.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations 
{
    public class CreateAccount : IWriteOperations<CreateAccountCommand>
    {
        public IEventSender eventSender;

        public CreateAccount(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }
        public void PerformOperation(CreateAccountCommand operation, Database database)
        {
            //Database database = Database.GetInstance();
            var person = database.GetPersonByCnp(operation.Cnp);
            if (person == null)
                throw new Exception("Costumer does not exist or CNP wrong");

            Account account = new Account();
            var random = new Random();
            account.Currency = operation.Currency;
            account.Type = operation.AccountType;
            account.IbanCode = random.Next(1000000).ToString();
            account.Balance = 0;
            account.Limit = operation.Limit;
            account.Status = "Active";
            person.Accounts.Add(account);
            database.Accounts.Add(account);

            database.SaveChange();
            AccountCreated eventAccCreated = new(operation.Name, operation.Cnp, account.IbanCode, operation.AccountType, account.Status);
            eventSender.SendEvent(eventAccCreated);

        }

    }
}
