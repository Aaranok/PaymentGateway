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
        public List<ServiceXTransaction> ServXTrans = new List<ServiceXTransaction>();

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
        public Person GetPersonByCnp(string cnp)
        {
            foreach(var item in this.Persons)
            {
                if (item.Cnp == cnp)
                    return item;
            }
            return null;
        }

        public Account GetAccountByInfo(string iban)//get by IBAN
        {
            foreach (var item in this.Accounts)
            {
                if (item.IbanCode == iban)
                    return item;
            }
            return null;
        }
        public string GetIbanByCnp(string cnp)
        {
            foreach (var item in this.Persons)
            {
                if (item.Cnp == cnp)
                    return item.Accounts[0].IbanCode;
                return null;
            }
            return null;
        }
        public Service GetServiceFromName(string name)
        {
            foreach (var item in this.Services)
            {
                if (item.Name == name)
                    return item;
            }
            return null;
        }
        public Service GetServiceFromId(int id)
        {
            foreach (var item in this.Services)
            {
                if (item.Id == id)
                    return item;
            }
            return null;
        }

        public Account GetAccountByInfo(int id)//get by ID
        {
            foreach (var item in this.Accounts)
            {
                if (item.AccountID == id)
                    return item;
            }
            return null;
        }

    }
}
