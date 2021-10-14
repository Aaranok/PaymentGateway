using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data
{
    public class Database
    {
        public List<Person> Persons = new();
        public List<Account> Accounts = new ();
        public List<Service> Services = new();
        public List<Transaction> Transactions = new();
        public List<ServiceXTransaction> ServXTrans = new();

        private static Database _instance;
        public static Database GetInstance()
        {
            if (_instance == null)
                _instance = new Database();
            return _instance;
        }

        public void SaveChange()
        {
            Console.WriteLine("Saved to database");
        }
    }
}
