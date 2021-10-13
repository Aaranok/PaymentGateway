using PaymentGateway.Abstractions;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;

namespace PaymentGateway.Application.WriteOperations 
{
    public class CreateAccount : IWriteOperations<CreateAccountCommand>
    {

        //public CreateAccount(IEventSender eventSender)
        //{
        //    this.eventSender = eventSender;
        //}
        private readonly IEventSender _eventSender;
        private readonly AccountOptions _accountOptions;
        private readonly Database _database;
        //private readonly NewIban _ibanService;~~~~~~~~~~make Iban Read
        public CreateAccount(IEventSender eventSender, AccountOptions accountOptions, Database database)
        {
            _eventSender = eventSender;
            _accountOptions = accountOptions;
            _database = database;
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
            _eventSender.SendEvent(eventAccCreated);
        }
        public void PerformOperation(CreateAccountCommand operation)
        {
            //Database database = Database.GetInstance();
            var persIdent = new PersonIdentifier(_database);
            var person =persIdent.GetPersonByCnp(operation.Cnp);
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
            _database.Accounts.Add(account);

            _database.SaveChange();
            AccountCreated eventAccCreated = new(operation.Name, operation.Cnp, account.IbanCode, operation.AccountType, account.Status);
            _eventSender.SendEvent(eventAccCreated);
        }

    }
}
