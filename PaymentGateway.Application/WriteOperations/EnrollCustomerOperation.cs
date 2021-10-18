﻿using PaymentGateway.Models;
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
        private readonly Data.PaymentDbContext _dbContext;
        public EnrollCustomerOperation(IMediator mediator, Data.PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(EnrollCustomerCommand request, CancellationToken cancellationToken)
        {
            var random = new Random();
            Person person = new()
            {
                Cnp = request.Cnp,
                Name = request.Name,
                //PersonID = _dbContext.Persons.Count + 1
            };
            if (request.ClientType == "Company")
                person.Type = (int)PersonType.Company;
            else if (request.ClientType == "Individual")
                person.Type = (int)PersonType.Individual;
            else
                throw new Exception("Unsupported Type");


            _dbContext.Persons.Add(person);

            Account account = new()
            {
                Type = request.AccountType,
                Currency = request.Currency,
                Balance = 0,
                IbanCode = random.Next(1000000).ToString(),
                PersonID = person.PersonID,
                Status = "Active"
            };
            _dbContext.Accounts.Add(account);

            _dbContext.SaveChanges();
            CustomerEnrolled eventCustEnroll = new(request.Name, request.Cnp, request.ClientType);
            await _mediator.Publish(eventCustEnroll, cancellationToken);
            return Unit.Value;
        }
    }
}
