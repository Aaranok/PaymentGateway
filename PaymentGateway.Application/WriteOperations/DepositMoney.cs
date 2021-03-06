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
    public class DepositMoney : IRequestHandler<DepositMoneyCommand>
    {
        private readonly Data.PaymentDbContext _dbContext;
        private readonly IMediator _mediator;

        public DepositMoney(IMediator mediator, Data.PaymentDbContext dbContext)//stuff to fix
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(DepositMoneyCommand request, CancellationToken cancellationToken)
        {
            var account = _dbContext.Accounts.FirstOrDefault(acc => acc.IbanCode == request.Iban);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            Transaction transaction = new()
            {
                AccountId = account.Id,
                Amount = request.Amount,
                Currency = request.Currency,
                DateOfTransaction = request.DateOfTransaction,
                Type = "Deposit"
            };
            transaction.DateOfOperation = transaction.DateOfTransaction.AddDays(2);

            account.Balance += transaction.Amount;
            _dbContext.Transactions.Add(transaction);
            DepositDone eventDepDone = new(request.Iban, request.Currency, request.Amount, request.DateOfOperation);
            _dbContext.SaveChanges();
            await _mediator.Publish(eventDepDone, cancellationToken);
            return Unit.Value;
        }
    }
}
