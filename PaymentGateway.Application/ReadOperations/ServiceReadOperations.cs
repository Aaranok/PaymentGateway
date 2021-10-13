using PaymentGateway.Data;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.ReadOperations
{ 
    public class ServiceReadOperations
    {
        private readonly Database _database;

        public Service GetServiceByName(string name)
        {
            return _database.Services.FirstOrDefault(service => service.Name == name);
            /*
            foreach (var item in _database.Services)
            {
                if (item.Name == name)
                    return item;
            }
            return null;*/
        } 
    }
}
