using PaymentGateway.Data;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.ReadOperations
{
    class PersonIdentifier
    {
        private readonly Database _database;
        public PersonIdentifier(Database database)
        {
            _database = database;
        }
        public Person GetPersonByCnp(string cnp)
        {
            foreach (var item in _database.Persons)
            {
                if (item.Cnp == cnp)
                    return item;
            }
            return null;
        }
    }
}
