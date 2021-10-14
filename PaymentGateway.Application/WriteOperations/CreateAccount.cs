using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
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
        private readonly Database _database;
        public CreateAccount(IMediator mediator, AccountOptions accountOptions, Database database)
        {
            _mediator = mediator;
            _accountOptions = accountOptions;
            _database = database;
        }
        
        public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var person = _database.Persons.FirstOrDefault(e => e.Cnp == request.Cnp);

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
            _database.Accounts.Add(account);

            _database.SaveChange();
            AccountCreated eventAccCreated = new(request.Name, request.Cnp, account.IbanCode, request.AccountType, account.Status);
            await _mediator.Publish(eventAccCreated, cancellationToken);
            return Unit.Value;
        }

    }
}
