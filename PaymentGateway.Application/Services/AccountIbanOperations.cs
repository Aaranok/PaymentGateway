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
        private readonly PaymentDbContext _dbContext;
        public AccountIbanOperations(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetIbanByCnp(string cnp)
        {

            foreach (var item in _dbContext.Persons)
            {
                if (item.Cnp == cnp)
                    return item.Accounts[0].IbanCode;
                return null;
            }
            return null;
        }
        public Account GetAccountByIban(string iban)
        {
            foreach (var item in _dbContext.Accounts)
            {
                if (item.IbanCode == iban)
                    return item;
            }
            return null;
        }
    }
}
