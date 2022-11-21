using System.Collections.Generic;
using System.Threading.Tasks;
using Rabobank.TechnicalTest.GCOB.Dtos;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetCustomers();

        public Task<Customer> InsertCustomer(Customer customer);
    }
}
