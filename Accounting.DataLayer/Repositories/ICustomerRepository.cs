using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acconting.ViewModles.Customers;

namespace Accounting.DataLayer.Repositories
{
    public interface ICustomerRepository
    {
        List<Customers> GetAllCustomers();
        IEnumerable<Customers> GetCustomersByFilter(string parameter);
        List<ListCustomerViewModle> GetNameCustomers(string filter = "");
        Customers GetCustomersById(int customerID);
        bool InsertCustomer(Customers  customer);
        bool UpdateCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int customerID);
        int GetCustomerIdByName(string name);
        string GetCustomerNameById(int customerId);

    }
}
