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
    public class WithdrawMoney : IWriteOperations<WithdrawMoneyCommand>
    {
        public IEventSender eventSender;

        public WithdrawMoney(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }
        public void PerformOperation(WithdrawMoneyCommand operation, Database database)
        {
            var account = database.GetAccountByInfo(operation.Iban);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            if (account.Balance < operation.Amount)
            {
                throw new Exception("Not enough funds");
            }

            Transaction transaction = new Transaction();
            transaction.Amount = -operation.Amount;
            transaction.Currency = operation.Currency;
            transaction.DateOfTransaction = operation.DateOfTransaction;
            transaction.DateOfOperation = transaction.GetOpDate();
            transaction.Type = "Withdraw";

            TransactionCreated eventTransCreated = new(operation.DateOfTransaction, transaction.Type, operation.Currency, operation.Amount, operation.Iban);
            eventSender.SendEvent(eventTransCreated);

            account.Balance -= transaction.Amount;
            WithdrawDone eventWitDone = new(operation.Iban, operation.Currency, operation.Amount, operation.DateOfOperation);
            eventSender.SendEvent(eventWitDone);
            database.SaveChange();

        }
    }
}
