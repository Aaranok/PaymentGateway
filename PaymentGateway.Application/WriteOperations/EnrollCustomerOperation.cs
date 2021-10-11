using PaymentGateway.Abstractions;
using PaymentGateway.Models;
using PaymentGateway.WriteSide;
using PaymentGateway.Data;
using System;
using PaymentGateway.PublishedLanguage.Events;

namespace PaymentGateway.Application.WriteOperations
{
    public class EnrollCustomerOperation : IWriteOperations<EnrollCustomerCommand>
    {
        public IEventSender eventSender;
        public EnrollCustomerOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }
        public void PerformOperation(EnrollCustomerCommand operation, Database database)
        {

            var random = new Random();
            //Database database = Database.GetInstance();
            Person person = new Person();
            person.Cnp = operation.Cnp;
            person.Name = operation.Name;

            if (operation.ClientType == "Company")
                person.Type = (int)PersonType.Company;
            else if (operation.ClientType == "Individual")
                person.Type = (int)PersonType.Individual;
            else
                throw new Exception("Unsupported Type");


            database.Persons.Add(person);

            Account account = new Account();
            account.Type = operation.AccountType;
            account.Currency = operation.Currency;
            account.Balance = 0;
            account.IbanCode = random.Next(1000000).ToString();
            database.Accounts.Add(account);

            database.SaveChange();
            CustomerEnrolled eventCustEnroll = new(operation.Name, operation.Cnp, operation.ClientType);
            eventSender.SendEvent(eventCustEnroll);

        }
    }
}
