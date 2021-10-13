using PaymentGateway.Abstractions;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using PaymentGateway.WriteSide;
using System;
using static PaymentGateway.Models.ServiceXTransaction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using System.IO;
using PaymentGateway.Application.ReadOperations;

namespace PaymentGateway
{
    class Program
    {
        static IConfiguration Configuration;
        static void Main(string[] args)
        {

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.RegisterBusinessServices(Configuration);

            services.AddSingleton<IEventSender, EventSender>();
            services.AddSingleton(Configuration);

            var serviceProvider = services.BuildServiceProvider();

            //Database DB = new Database();
            var DB = serviceProvider.GetRequiredService<Database>();


            var customer1 = new EnrollCustomerCommand();
            customer1.Name = "Gigi";
            customer1.Currency = "$";
            customer1.Cnp = "1234141341124";
            customer1.ClientType = "Individual";
            customer1.AccountType = "Credit";

            var enrollOp1 = serviceProvider.GetRequiredService<EnrollCustomerOperation>();
            enrollOp1.PerformOperation(customer1);

            CreateAccountCommand account1 = new CreateAccountCommand();
            account1.AccountType = "Credit";
            account1.Cnp = "1234141341124";
            account1.Currency = "$";
            account1.Limit = 10000;
            account1.Name = "Gigi";

            var createAccount1 = serviceProvider.GetRequiredService<CreateAccount>();
            createAccount1.PerformOperation(account1);

            DepositMoneyCommand depMoney = new DepositMoneyCommand();
            depMoney.DateOfTransaction = DateTime.UtcNow.AddDays(-3);
            depMoney.DateOfOperation = DateTime.UtcNow;
            depMoney.Currency = "$";
            depMoney.Amount = 300;
            //depMoney.Iban = DB.GetIbanByCnp(account1.Cnp);
            AccountIbanOperations getIbanOp = new AccountIbanOperations(DB);
            depMoney.Iban = getIbanOp.GetIbanByCnp(account1.Cnp);

            DepositMoney depMoneyEvent1 = serviceProvider.GetRequiredService<DepositMoney>();
            depMoneyEvent1.PerformOperation(depMoney);

            WithdrawMoneyCommand witMoneyCmd = new WithdrawMoneyCommand();
            witMoneyCmd.Amount = 100;
            witMoneyCmd.Currency = "$";
            witMoneyCmd.DateOfTransaction = DateTime.UtcNow.AddDays(-2);
            witMoneyCmd.DateOfOperation = DateTime.UtcNow;
            //witMoneyCmd.Iban = DB.GetIbanByCnp(account1.Cnp);
            //AccountIbanOperations getIbanOp = new AccountIbanOperations(DB);
            depMoney.Iban = getIbanOp.GetIbanByCnp(account1.Cnp);

            WithdrawMoney withdrawMoneyEvent = serviceProvider.GetRequiredService<WithdrawMoney>();
            withdrawMoneyEvent.PerformOperation(witMoneyCmd);

            CreateServiceCommand createServCmd = new CreateServiceCommand();
            createServCmd.Limit = 50;
            createServCmd.Name = "Schema";
            createServCmd.Value = 200;
            createServCmd.Currency = "$";

            CreateService createServiceEvent = serviceProvider.GetRequiredService<CreateService>();
            createServiceEvent.PerformOperation(createServCmd);

            CreateServiceCommand createServCmd2 = new CreateServiceCommand();
            createServCmd2.Limit = 2;
            createServCmd2.Name = "Schema2";
            createServCmd2.Value = 50;
            createServCmd2.Currency = "$";

            CreateService createServiceEvent2 = serviceProvider.GetRequiredService<CreateService>();
            createServiceEvent2.PerformOperation(createServCmd2);

            PurchaseServiceCommand purchaseService = new PurchaseServiceCommand();
            //purchaseService.Iban = DB.GetIbanByCnp(account1.Cnp);
            purchaseService.Iban = getIbanOp.GetIbanByCnp(account1.Cnp);
            purchaseService.personName = "Gigi";
            purchaseService.DateOfTransaction = DateTime.UtcNow;

            ServiceList listItem;
            Service aux = new Service();
            //aux = DB.GetServiceFromName("Schema");
            ServiceReadOperations servRead = new ServiceReadOperations();
            aux = servRead.GetServiceByName("Schema");
            listItem.IdService = aux.Id;
            listItem.NoPurchased = 2;
            purchaseService.Product.Add(listItem);
            //aux = DB.GetServiceFromName("Schema2");
            aux = servRead.GetServiceByName("Schema2");
            listItem.IdService = aux.Id;
            listItem.NoPurchased = 2;
            purchaseService.Product.Add(listItem);

            PurchaseService purchaseServiceEvent = serviceProvider.GetRequiredService<PurchaseService>();
            purchaseServiceEvent.PerformOperation(purchaseService);

            var query = new Application.ReadOperations.ListOfAccounts.Query
            {
                PersonId = 1
            };

            var handler = serviceProvider.GetRequiredService<ListOfAccounts.QueryHandler>();
            var result = handler.PerformOperation(query);

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~~~~~~~~~~~~~~~~~~~         ZA OLD TINGS            ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            /*  Database DB = new Database(); 
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
            */
        }
    }
}
