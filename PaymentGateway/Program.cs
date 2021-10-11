using PaymentGateway.Abstractions;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.WriteSide;
using System;
using System.Collections.Generic;

namespace PaymentGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            EnrollCustomerCommand customer1 = new EnrollCustomerCommand();
            customer1.Name = "Gigi";
            customer1.Currency = "$";
            customer1.Cnp = "1234141341124";
            customer1.ClientType = "Individual";
            customer1.AccountType = "Credit";

            IEventSender eventSender = new EventSender();

            EnrollCustomerOperation enrollOp1 = new EnrollCustomerOperation(eventSender);
            enrollOp1.PerformOperation(customer1);



        }
    }
}
