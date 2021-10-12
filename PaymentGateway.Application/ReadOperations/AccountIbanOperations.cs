using PaymentGateway.Data;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.ReadOperations
{
    public class AccountIbanOperations
    {
        private readonly Database _database;
        public AccountIbanOperations(Database database)
        {
            _database = database;
        }

        public string GetIbanByCnp(string cnp)
        {
            foreach (var item in _database.Persons)
            {
                if (item.Cnp == cnp)
                    return item.Accounts[0].IbanCode;
                return null;
            }
            return null;
        }
        public Account GetAccountByIban(string iban)
        {
            foreach (var item in _database.Accounts)
            {
                if (item.IbanCode == iban)
                    return item;
            }
            return null;
        }
    }
}
