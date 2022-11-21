using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Repositories;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICountryRepository _countryRepository;

        public CustomerService(ICustomerRepository customerRepository, IAddressRepository addressRepository, ICountryRepository countryRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customerDtos = (await _customerRepository.GetAllAsync()).ToList();
            var addressDtos = (await _addressRepository.GetAllAsync()).ToList();
            var countryDtos = (await _countryRepository.GetAllAsync()).ToList();

            var customers = from c in customerDtos
                join a in addressDtos on c.AddressId equals a.Id
                join coun in countryDtos on a.CountryId equals coun.Id
                select new Customer()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Street = a.Street,
                    City = a.City,
                    Postcode = a.Postcode,
                    Country = coun.Name
                };

return customers;
        }

        public async Task<Customer> InsertCustomer(Customer customer)
        {
            if (customer.Id > 0 || customer.Id < 0)
                throw new Exception("Please pass empty customer id");
            customer.Id = await _customerRepository.GenerateIdentityAsync();


            AddressDto address = new AddressDto
            {
                Id = await _addressRepository.GenerateIdentityAsync(),
                Street = customer.Street,
                City = customer.City,
                Postcode = customer.Postcode,
                CountryId = (await _countryRepository.GetAllAsync()).FirstOrDefault(s => s.Name.Equals(customer.Country, StringComparison.CurrentCultureIgnoreCase))!.Id
            };

            CustomerDto customerDto = new CustomerDto
            {
                Id = await _customerRepository.GenerateIdentityAsync(),
                AddressId = address.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

    
            await _addressRepository.InsertAsync(address);
            await _customerRepository.InsertAsync(customerDto);
            return customer;
        }
    }
}
