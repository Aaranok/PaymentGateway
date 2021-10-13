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
        private readonly IEventSender _eventSender;
        private readonly Database _database;
        public EnrollCustomerOperation(IEventSender eventSender, Database database)
        {
            this._eventSender = eventSender;
            _database = database;
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
            _eventSender.SendEvent(eventCustEnroll);

        }

        public void PerformOperation(EnrollCustomerCommand operation)
        {
            var random = new Random();
            Person person = new Person();
            person.Cnp = operation.Cnp;
            person.Name = operation.Name;
            person.PersonID = _database.Persons.Count + 1;
            //person.PersonID = 1;
            if (operation.ClientType == "Company")
                person.Type = (int)PersonType.Company;
            else if (operation.ClientType == "Individual")
                person.Type = (int)PersonType.Individual;
            else
                throw new Exception("Unsupported Type");


            _database.Persons.Add(person);

            Account account = new Account();
            account.Type = operation.AccountType;
            account.Currency = operation.Currency;
            account.Balance = 0;
            account.IbanCode = random.Next(1000000).ToString();
            account.PersonID = person.PersonID;
            _database.Accounts.Add(account);

            _database.SaveChange();
            CustomerEnrolled eventCustEnroll = new(operation.Name, operation.Cnp, operation.ClientType);
            _eventSender.SendEvent(eventCustEnroll);
        }
    }
}
