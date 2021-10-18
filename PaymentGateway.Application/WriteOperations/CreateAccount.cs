using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;


namespace PaymentGateway.Application.WriteOperations 
{
    public class CreateAccount : IRequestHandler<CreateAccountCommand>
    {
        private readonly IMediator _mediator;
        private readonly AccountOptions _accountOptions;
        private readonly Data.PaymentDbContext _dbContext;
        public CreateAccount(IMediator mediator, AccountOptions accountOptions, Data.PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _accountOptions = accountOptions;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var person = _dbContext.People.FirstOrDefault(e => e.Cnp == request.Cnp);

            if (person == null)
                throw new Exception("Costumer does not exist or CNP wrong");

            var random = new Random();
            Account account = new()
            {
                Currency = request.Currency,
                Type = request.AccountType,
                IbanCode = random.Next(1000000).ToString(),
                Balance = 0,
                Limit = request.Limit,
                Status = "Active"
            };
            person.Accounts.Add(account);
            _dbContext.Accounts.Add(account);

            _dbContext.SaveChanges();
            AccountCreated eventAccCreated = new(request.Name, request.Cnp, account.IbanCode, request.AccountType, account.Status);
            await _mediator.Publish(eventAccCreated, cancellationToken);
            return Unit.Value;
        }

    }
}
