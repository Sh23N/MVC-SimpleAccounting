using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acconting.ViewModles.Customers;
using Accounting.DataLayer.Repositories;
//using System.Data.Entity.Infrastructure;

namespace Accounting.DataLayer.Repositories.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private Accounting_DBEntities db;

        public CustomerRepository(Accounting_DBEntities context) //dependency injection by constructor
        {
            db = context;
        }
        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();
        }
        public IEnumerable<Customers> GetCustomersByFilter(string parameter)
        {
            return db.Customers.Where(c => c.FullName.Contains(parameter) || c.Email.Contains(parameter) || c.Mobile.Contains(parameter)).ToList();
        }

        public List<ListCustomerViewModle> GetNameCustomers(string filter = "")
        {
            if (filter == "")
            {
                return db.Customers.Select(c => new ListCustomerViewModle()
                {
                    CustomerID = c.CustomersID,
                    FullName = c.FullName
                }).ToList();

            }
            return db.Customers.Where(c => c.FullName.Contains(filter)).Select(c => new ListCustomerViewModle()
            {
                CustomerID = c.CustomersID,
                FullName = c.FullName
            }).ToList();
        }

        public Customers GetCustomersById(int customerID)
        {
            return db.Customers.Find(customerID);
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCustomer(Customers customer)
        {
            var local = db.Set<Customers>().Local
                .FirstOrDefault(f => f.CustomersID == customer.CustomersID);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(customer).State = EntityState.Modified;
            return true;

        }

        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerID)
        {
            try
            {
                var customer = GetCustomersById(customerID);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetCustomerIdByName(string name)
        {
            return db.Customers.First(c => c.FullName == name).CustomersID;
        }

        public string GetCustomerNameById(int customerId)
        {
            return db.Customers.Find(customerId).FullName;
        }
    }
}
