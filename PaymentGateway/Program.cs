using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using System.IO;
using PaymentGateway.Application.ReadOperations;
using System.Threading.Tasks;
using System.Threading;
using PaymentGateway.PublishedLanguage.Events;
using FluentValidation;
using System.Linq;

namespace PaymentGateway
{
    class Program
    {
        static IConfiguration Configuration;
        static async Task Main(string[] args)
        {

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;
            services.RegisterBusinessServices(Configuration);
            services.AddPaymentDataAccess(Configuration);


            //services.AddMediatR(typeof(ListOfAccounts).Assembly, typeof(AllEventsHandler).Assembly);
            services.Scan(scan => scan
                .FromAssemblyOf<ListOfAccounts>()
                .AddClasses(classes => classes.AssignableTo<IValidator>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddMediatR(new[] { typeof(ListOfAccounts).Assembly, typeof(AllEventsHandler).Assembly }); // get all IRequestHandler and INotificationHandler classes

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            services.AddScopedContravariant<INotificationHandler<INotification>, AllEventsHandler>(typeof(CustomerEnrolled).Assembly);

            services.AddMediatR(new[] { typeof(ListOfAccounts).Assembly, typeof(AllEventsHandler).Assembly }); // get all IRequestHandler and INotificationHandler classes

            services.AddSingleton(Configuration);

            var serviceProvider = services.BuildServiceProvider();

            var DB = serviceProvider.GetRequiredService<PaymentDbContext>();


            var mediator = serviceProvider.GetRequiredService<IMediator>();

            EnrollCustomerCommand customer1 = new()
            {
                Name = "Gigi",
                Currency = "$",
                Cnp = "1234141341124",
                ClientType = "Individual",
                AccountType = "Credit"
            };

            await mediator.Send(customer1, cancellationToken);

            CreateAccountCommand account1 = new()
            {
                AccountType = "Credit",
                Cnp = "1234141341124",
                Currency = "$",
                Limit = 10000,
                Name = "Gigi"
            };

            await mediator.Send(account1, cancellationToken);

            DepositMoneyCommand depMoney = new()
            {
                DateOfTransaction = DateTime.UtcNow.AddDays(-3),
                DateOfOperation = DateTime.UtcNow,
                Currency = "$",
                Amount = 300
            };
            AccountIbanOperations getIbanOp = new(DB);
            //depMoney.Iban = getIbanOp.GetIbanByCnp(account1.Cnp);//fix this // probably fixed
            var auxPers = DB.People.FirstOrDefault(pers => pers.Cnp == account1.Cnp);
            var auxAcc = DB.Accounts.FirstOrDefault(Acc => Acc.PersonId == auxPers.Id);

            depMoney.Iban = auxAcc.IbanCode;

            await mediator.Send(depMoney, cancellationToken);

            WithdrawMoneyCommand witMoneyCmd = new()
            {
                Amount = 100,
                Currency = "$",
                DateOfTransaction = DateTime.UtcNow.AddDays(-2),
                DateOfOperation = DateTime.UtcNow,
        };
            var auxPers2 = DB.People.FirstOrDefault(pers => pers.Cnp == account1.Cnp);
            var auxAcc2 = DB.Accounts.FirstOrDefault(Acc => Acc.PersonId == auxPers.Id);
            witMoneyCmd.Iban = auxAcc2.IbanCode;

            await mediator.Send(witMoneyCmd, cancellationToken);

            CreateServiceCommand createServCmd = new()
            {
                Limit = 50,
                Name = "Schema",
                Value = 200,
                Currency = "$"
            };

            await mediator.Send(createServCmd, cancellationToken);

            CreateServiceCommand createServCmd2 = new()
            {
                Limit = 2,
                Name = "Schema2",
                Value = 50,
                Currency = "$"
            };

            await mediator.Send(createServCmd2, cancellationToken);
            /*
            PurchaseServiceCommand purchaseService = new()
            {
                Iban = getIbanOp.GetIbanByCnp(account1.Cnp),
                PersonName = "Gigi",
                DateOfTransaction = DateTime.UtcNow
            };

            ServiceList listItem;
            Service aux = new();
            aux = DB.Services.FirstOrDefault(service => service.Name == "Schema");

            listItem.IdService = aux.Id;
            listItem.NoPurchased = 2;
            purchaseService.Product.Add(listItem);
            aux = DB.Services.FirstOrDefault(service => service.Name == "Schema2");

            listItem.IdService = aux.Id;
            listItem.NoPurchased = 2;
            purchaseService.Product.Add(listItem);

            await mediator.Send(purchaseService, cancellationToken);
            */
            var query = new ListOfAccounts.Query
            {
               PersonId = 1
            };

            var result = await mediator.Send(query, cancellationToken);
        }
    }
}
