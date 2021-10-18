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
        private readonly PaymentDbContext _dbContext;
        private readonly IMediator _mediator;

        public CreateService(IMediator mediator, PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            Service service = new()
            {
                Value = request.Value,
                Name = request.Name,
                Limit = request.Limit,
                Currency = request.Currency
            };

            _dbContext.Services.Add(service);
            _dbContext.SaveChanges();
            ServiceCreated eventServCreated = new(request.Name, request.Value, request.Limit, request.Currency);
            await _mediator.Publish(eventServCreated, cancellationToken);

            return Unit.Value;

        }
    }
}
