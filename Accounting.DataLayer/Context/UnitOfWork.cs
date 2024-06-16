using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Repositories.Services;

namespace Accounting.DataLayer.Context
{
    public class UnitOfWork : IDisposable
    {
        private Accounting_DBEntities db = new Accounting_DBEntities();
        private ICustomerRepository _customerRepository;

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new CustomerRepository(db);
                return _customerRepository;
            }
        }

        private GenericRepository<Accounting> _accountingRepository;

        public GenericRepository<Accounting> AccGenericRepository
        {
            get
            {
                if (_accountingRepository == null)
                {
                    _accountingRepository=new GenericRepository<Accounting>(db);
                }

                return _accountingRepository;
            }
        }

        public GenericRepository<Login> _LoginRepository;

        public GenericRepository<Login> LoginRepository
        {
            get
            {
                if (_LoginRepository == null)
                {
                    _LoginRepository=new GenericRepository<Login>(db);
                }

                return _LoginRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public void Dispose()//get all memory from db
        {
            db.Dispose();
        }
    }
}
