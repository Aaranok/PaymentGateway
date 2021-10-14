using PaymentGateway.Data;
using PaymentGateway.Models;
using System.Linq;

namespace PaymentGateway.Application.ReadOperations
{ 
    public class ServiceReadOperations
    {
        private readonly Database _database;

        public Service GetServiceByName(string name)
        {
            return _database.Services.FirstOrDefault(service => service.Name == name);

        } 
    }
}
