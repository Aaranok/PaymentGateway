using PaymentGateway.Abstractions;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using PaymentGateway.WriteSide;
using System;
using System.Collections.Generic;
using static PaymentGateway.Models.ServiceXTransaction;

namespace PaymentGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            Database DB = new Database(); 
            EnrollCustomerCommand customer1 = new EnrollCustomerCommand();
            customer1.Name = "Gigi";
            customer1.Currency = "$";
            customer1.Cnp = "1234141341124";
            customer1.ClientType = "Individual";
            customer1.AccountType = "Credit";

            IEventSender eventSender = new EventSender();

            EnrollCustomerOperation enrollOp1 = new EnrollCustomerOperation(eventSender);
            enrollOp1.PerformOperation(customer1, DB);

            CreateAccountCommand account1 = new CreateAccountCommand();
            account1.AccountType = "Credit";
            account1.Cnp = "1234141341124";
            account1.Currency = "$";
            account1.Limit = 10000;
            account1.Name = "Gigi";

            CreateAccount createAccount1 = new CreateAccount(eventSender);
            createAccount1.PerformOperation(account1, DB);

            DepositMoneyCommand depMoney = new DepositMoneyCommand();
            depMoney.DateOfTransaction = DateTime.UtcNow.AddDays(-3);
            depMoney.DateOfOperation = DateTime.UtcNow;
            depMoney.Currency = "$";
            depMoney.Amount = 300;
            depMoney.Iban = DB.GetIbanByCnp(account1.Cnp);

            DepositMoney depMoneyEvent1 = new DepositMoney(eventSender);
            depMoneyEvent1.PerformOperation(depMoney, DB);

            WithdrawMoneyCommand witMoneyCmd = new WithdrawMoneyCommand();
            witMoneyCmd.Amount = 100;
            witMoneyCmd.Currency = "$";
            witMoneyCmd.DateOfTransaction = DateTime.UtcNow.AddDays(-2);
            witMoneyCmd.DateOfOperation = DateTime.UtcNow;
            witMoneyCmd.Iban = DB.GetIbanByCnp(account1.Cnp);

            WithdrawMoney withdrawMoneyEvent = new WithdrawMoney(eventSender);
            withdrawMoneyEvent.PerformOperation(witMoneyCmd, DB);

            CreateServiceCommand createServCmd = new CreateServiceCommand();
            createServCmd.Limit = 50;
            createServCmd.Name = "Schema";
            createServCmd.Value = 200;
            createServCmd.Currency = "$";

            CreateService createServiceEvent = new CreateService(eventSender);
            createServiceEvent.PerformOperation(createServCmd, DB);

            CreateServiceCommand createServCmd2 = new CreateServiceCommand();
            createServCmd2.Limit = 2;
            createServCmd2.Name = "Schema2";
            createServCmd2.Value = 50;
            createServCmd2.Currency = "$";

            CreateService createServiceEvent2 = new CreateService(eventSender);
            createServiceEvent2.PerformOperation(createServCmd2, DB);

            PurchaseServiceCommand purchaseService = new PurchaseServiceCommand();
            purchaseService.Iban = DB.GetIbanByCnp(account1.Cnp);
            purchaseService.personName = "Gigi";
            purchaseService.DateOfTransaction = DateTime.UtcNow;
            
            ServiceList listItem;
            Service aux = new Service();
            aux = DB.GetServiceFromName("Schema");
            listItem.IdService = aux.Id;
            listItem.NoPurchased = 2;
            purchaseService.Product.Add(listItem);
            aux = DB.GetServiceFromName("Schema2");
            listItem.IdService = aux.Id;
            listItem.NoPurchased = 2;
            purchaseService.Product.Add(listItem);

            PurchaseService purchaseServiceEvent = new PurchaseService(eventSender);
            purchaseServiceEvent.PerformOperation(purchaseService, DB);

        }
    }
}
