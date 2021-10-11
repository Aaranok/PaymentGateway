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
        public List<Person> Persons = new List<Person>();
        public List<Account> Accounts = new List<Account>();
        public List<Service> Services = new List<Service>();
        public List<Transaction> Transactions = new List<Transaction>();

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
