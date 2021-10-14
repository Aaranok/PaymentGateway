using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace PaymentGateway.Application.WriteOperations
{
    public class CreateService : IRequestHandler<CreateServiceCommand>
    {
        private readonly Database _database;
        private readonly IMediator _mediator;

        public CreateService(IMediator mediator, Database database)
        {
            _mediator = mediator;
            _database = database;
        }

        public async Task<Unit> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            Service service = new Service
            {
                Value = request.Value,
                Name = request.Name,
                Limit = request.Limit,
                Currency = request.Currency
            };

            _database.Services.Add(service);
            _database.SaveChange();
            ServiceCreated eventServCreated = new(request.Name, request.Value, request.Limit, request.Currency);
            await _mediator.Publish(eventServCreated, cancellationToken);

            return Unit.Value;

        }
    }
}
