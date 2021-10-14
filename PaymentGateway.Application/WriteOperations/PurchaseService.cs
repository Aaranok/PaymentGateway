using MediatR;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{

    public class PurchaseService: IRequestHandler<PurchaseServiceCommand>
    {
        private readonly Database _database;
        private readonly IMediator _mediator;

        public PurchaseService(IMediator mediator, Database database)
        {
            _mediator = mediator;
            _database = database;
        }

        public Task<Unit> Handle(PurchaseServiceCommand request, CancellationToken cancellationToken)
        {
            var accountIdent = new AccountIbanOperations(_database);
            var account = accountIdent.GetAccountByIban(request.Iban);

            if (account == null)
            {
                throw new Exception("Account not found");
            }
            double totalValue = 0;
            string currency = "";
            foreach (var item in request.Product)
            {
                var service = _database.Services.FirstOrDefault(service => service.Id == item.IdService);

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

            if (totalValue > account.Balance)
            {
                throw new Exception("Not enough capital");
            }

            Transaction transaction = new()
            {
                Amount = totalValue,
                Currency = currency,
                DateOfTransaction = request.DateOfTransaction,
                Value = totalValue,
                Type = "Purchase"
        };
            transaction.DateOfOperation = transaction.GetOpDate();
            transaction.Id = _database.Transactions.Count() + 1;

            _database.Transactions.Add(transaction);

            account.Balance -= totalValue;

            foreach (var item in request.Product)
            {
                ServiceXTransaction servXTransItem = new()
                {
                    IdTransaction = transaction.Id
                };
                servXTransItem.ServiceIdList.IdService = item.IdService;
                servXTransItem.ServiceIdList.NoPurchased = item.NoPurchased;
                _database.ServXTrans.Add(servXTransItem);
            }

            _database.SaveChange();
            ServicePurchased eventServPurchased = new(request.Iban, request.Cnp, request.PersonName, request.Product);
            return Unit.Task;

        }
    }
}
