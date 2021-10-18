using PaymentGateway.Data;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace PaymentGateway.Application.ReadOperations
{
    public class ListOfAccounts
    {
        public class Validator : AbstractValidator<Query>
        {

            public Validator(Data.PaymentDbContext _dbContext)
            {
                RuleFor(q => q).Must(query =>
                {
                    var person = query.PersonId.HasValue ?
                    _dbContext.People.FirstOrDefault(x => x.Id == query.PersonId) :
                    _dbContext.People.FirstOrDefault(x => x.Cnp == query.Cnp);

                    return person != null;
                }).WithMessage("Customer not found");
            }
        }

        public class Validator2 : AbstractValidator<Query>
        {
            public Validator2(Data.PaymentDbContext _dbContext)
            {
                RuleFor(q => q).Must(q =>
                {
                    return q.PersonId.HasValue || !string.IsNullOrEmpty(q.Cnp);
                }).WithMessage("Person data is invalid - ");

                RuleFor(q => q.Cnp).Must(cnp =>
                {
                   if (string.IsNullOrEmpty(cnp))
                   {
                       return true;
                   }
                return cnp.Length == 13;
                }).WithMessage("CNP not 13 characters long");

                RuleFor(q => q.PersonId).Must(personId => 
                {
                    if (!personId.HasValue)
                    {
                        return true;//why?
                    }
                return personId.Value > 0;
            }).WithMessage("Person id is not positive");
        }
        }
            public class Query : IRequest<List<Model>>
            {
                public int? PersonId { get; set; }
                public string Cnp { get; set; }
            }

            public class QueryHandler : IRequestHandler<Query, List<Model>>
            {
                private readonly Data.PaymentDbContext _dbContext;

                public QueryHandler(Data.PaymentDbContext dbContext)
                {
                    _dbContext = dbContext;
                }

                public Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
                {

                    var person = request.PersonId.HasValue ?
                       _dbContext.People.FirstOrDefault(x => x.Id == request.PersonId) :
                       _dbContext.People.FirstOrDefault(x => x.Cnp == request.Cnp);

                    var db = _dbContext.Accounts.Where(x => x.PersonId == person.Id);
                    var result = db.Select(x => new Model
                    {
                        Balance = x.Balance,
                        Currency = x.Currency,
                        Iban = x.IbanCode,
                        Id = x.Id,
                        Limit = x.Limit,
                        Status = x.Status,
                        Type = x.Type
                    }).ToList();
                    return Task.FromResult(result);
                }
            }
            public class Model
            {
                public int Id { get; set; }
                public decimal Balance { get; set; }
                public string Currency { get; set; }
                public string Iban { get; set; }
                public string Status { get; set; }
                public decimal? Limit { get; set; }
                public string Type { get; set; }
            }
        }
    }