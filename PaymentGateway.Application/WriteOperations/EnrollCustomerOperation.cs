using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using PaymentGateway.Data;
using System;
using PaymentGateway.PublishedLanguage.Events;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace PaymentGateway.Application.WriteOperations
{
    public class EnrollCustomerOperation : IRequestHandler<EnrollCustomerCommand>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public EnrollCustomerOperation(IMediator mediator, Database database)
        {
            _mediator = mediator;
            _database = database;
        }
        public async Task<Unit> Handle(EnrollCustomerCommand request, CancellationToken cancellationToken)
        {
            var random = new Random();
            Person person = new Person();
            person.Cnp = request.Cnp;
            person.Name = request.Name;
            person.PersonID = _database.Persons.Count + 1;
            if (request.ClientType == "Company")
                person.Type = (int)PersonType.Company;
            else if (request.ClientType == "Individual")
                person.Type = (int)PersonType.Individual;
            else
                throw new Exception("Unsupported Type");


            _database.Persons.Add(person);

            Account account = new Account();
            account.Type = request.AccountType;
            account.Currency = request.Currency;
            account.Balance = 0;
            account.IbanCode = random.Next(1000000).ToString();
            account.PersonID = person.PersonID;
            _database.Accounts.Add(account);

            _database.SaveChange();
            CustomerEnrolled eventCustEnroll = new(request.Name, request.Cnp, request.ClientType);
            await _mediator.Publish(eventCustEnroll, cancellationToken);
            return Unit.Value;
        }
    }
}
