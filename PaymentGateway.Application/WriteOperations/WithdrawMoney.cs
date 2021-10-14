using MediatR;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    public class WithdrawMoney : IRequestHandler<WithdrawMoneyCommand>
    {
        private readonly Database _database;
        private readonly IMediator _mediator;

        public WithdrawMoney(IMediator mediator, Database database)
        {
            _mediator = mediator;
            _database = database;
        }

        public Task<Unit> Handle(WithdrawMoneyCommand request, CancellationToken cancellationToken)
        {
            var accountIdent = new AccountIbanOperations(_database);
            var account = _database.Accounts.FirstOrDefault(acc => acc.IbanCode == request.Iban);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            if (account.Balance < request.Amount)
            {
                throw new Exception("Not enough funds");
            }

            Transaction transaction = new Transaction();
            transaction.Amount = -request.Amount;
            transaction.Currency = request.Currency;
            transaction.DateOfTransaction = request.DateOfTransaction;
            transaction.DateOfOperation = transaction.GetOpDate();
            transaction.Type = "Withdraw";

            //TransactionCreated eventTransCreated = new(request.DateOfTransaction, transaction.Type, request.Currency, request.Amount, request.Iban);
            //_eventSender.SendEvent(eventTransCreated);

            account.Balance -= transaction.Amount;
            WithdrawDone eventWitDone = new(request.Iban, request.Currency, request.Amount, request.DateOfOperation);
            //_eventSender.SendEvent(eventWitDone);
            _database.SaveChange();
            return Unit.Task;
        }
    }
}
