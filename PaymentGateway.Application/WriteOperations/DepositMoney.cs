﻿using PaymentGateway.Abstractions;
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
    public class DepositMoney: IWriteOperations<DepositMoneyCommand>
    {
        public IEventSender eventSender;

        public DepositMoney(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void PerformOperation(DepositMoneyCommand operation, Database database)
        {
            //Database database = Database.GetInstance();
            var account = database.GetAccountByInfo(operation.Iban);
            if(account == null)
            {
                throw new Exception("Account not found");
            }

            Transaction transaction = new Transaction();
            transaction.Amount = operation.Amount;
            transaction.Currency = operation.Currency;
            transaction.DateOfTransaction = operation.DateOfTransaction;
            transaction.DateOfOperation = transaction.GetOpDate();
            transaction.Type = "Deposit";

            //database.SaveChange();

            TransactionCreated eventTransCreated = new(operation.DateOfTransaction, transaction.Type, operation.Currency, operation.Amount, operation.Iban);
            eventSender.SendEvent(eventTransCreated);


            //if (DateTime.UtcNow >= transaction.DateOfOperation)
            //{
            //    account.Balance += transaction.Amount;
            //    DepositDone eventDepDone = new(operation.Iban, operation.Currency,operation.Amount, operation.DateOfOperation);
            //    eventSender.SendEvent(eventDepDone);
            //}
            account.Balance += transaction.Amount;
            DepositDone eventDepDone = new(operation.Iban, operation.Currency, operation.Amount, operation.DateOfOperation);
            eventSender.SendEvent(eventDepDone);
            database.SaveChange();


        }

        public void PerformOperation(DepositMoneyCommand operation)
        {
           // throw new NotImplementedException();
        }
    }
}
