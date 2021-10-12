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
        public IEventSender eventSender;

        public CreateService(IEventSender eventSender)
        {
            this.eventSender = eventSender;
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
            eventSender.SendEvent(eventServCreated);
        }

        public void PerformOperation(CreateServiceCommand operation)
        {
            //throw new NotImplementedException();
        }
    }
}
