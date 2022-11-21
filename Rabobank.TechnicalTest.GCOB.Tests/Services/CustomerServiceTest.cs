using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerServiceTest
    {

        private MockRepository _mockRepository;

        private Mock<ICustomerRepository> _mockCustomerRepository;
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<ICountryRepository> _mockCountryRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this._mockRepository = new MockRepository(MockBehavior.Loose);

            this._mockCustomerRepository = this._mockRepository.Create<ICustomerRepository>();
            this._mockAddressRepository = this._mockRepository.Create<IAddressRepository>();
            this._mockCountryRepository = this._mockRepository.Create<ICountryRepository>();

            List<CountryDto> countryDtos = new List<CountryDto>
            {
                new CountryDto { Id = 1, Name = "Netherlands" },
                new CountryDto { Id = 2, Name = "Poland" },
                new CountryDto { Id = 3, Name = "Ireland" },
                new CountryDto { Id = 4, Name = "South Afrcia" },
                new CountryDto { Id = 5, Name = "India" }
            };

            _mockCountryRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(countryDtos);

            List<AddressDto> addressDtos = new List<AddressDto>
            {
               new AddressDto { Id = 1, Street = "Churchill-laan", City = "AMSTERDAM", Postcode = "1078 GA", CountryId = 1}
            };

            _mockAddressRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(addressDtos);


            List<CustomerDto> customerDtos = new List<CustomerDto>()
            {
                new CustomerDto {Id = 1, FirstName = "John", LastName = "Smith", AddressId = 1}
            };
            _mockCustomerRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(customerDtos);

        }

        private CustomerService CreateService()
        {
            return new CustomerService(
                this._mockCustomerRepository.Object,
                this._mockAddressRepository.Object,
                this._mockCountryRepository.Object);
        }


        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsReturned()
        {

            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.GetCustomers();

            // Assert
            Assert.IsTrue(result.Any());
            this._mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GivenInsertACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsIOnSerted_AndTheCustomerIsReturned()
        {
            // Arrange
            var service = this.CreateService();
            Customer customer = new Customer
            {
                FirstName = "Nick",
                LastName = "Jon",
                Street = "Address line1",
                City = "Test City",
                Postcode = "Test Postcode",
                Country = "Netherlands"
            };
            _mockAddressRepository.Setup(x => x.GenerateIdentityAsync()).ReturnsAsync(1);
            _mockCustomerRepository.Setup(x => x.GenerateIdentityAsync()).ReturnsAsync(2);

            // Act
            var result = await service.InsertCustomer(customer);

            // Assert
            Assert.IsTrue(result.Id > 0);

        }
    }
}