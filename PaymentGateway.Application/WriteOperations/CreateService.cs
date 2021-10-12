using PaymentGateway.Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    public class CreateService : IWriteOperations<CreateServiceCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;

        public CreateService(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;
        }
        public void PerformOperation(CreateServiceCommand operation, Database database)
        {
            Service service = new Service();

            service.Value = operation.Value;
            service.Name = operation.Name;
            service.Limit = operation.Limit;
            service.Currency = operation.Currency;

            database.Services.Add(service);
            database.SaveChange();
            ServiceCreated eventServCreated = new(operation.Name, operation.Value, operation.Limit, operation.Currency);
            _eventSender.SendEvent(eventServCreated);
        }

        public void PerformOperation(CreateServiceCommand operation)
        {
            Service service = new Service
            {
                Value = operation.Value,
                Name = operation.Name,
                Limit = operation.Limit,
                Currency = operation.Currency
            };

            _database.Services.Add(service);
            _database.SaveChange();
            ServiceCreated eventServCreated = new(operation.Name, operation.Value, operation.Limit, operation.Currency);
            _eventSender.SendEvent(eventServCreated);
        }
    }
}
