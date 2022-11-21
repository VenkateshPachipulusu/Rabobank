using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Rabobank.TechnicalTest.GCOB.Controllers;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Services;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerControllerTest
    {
        private Mock< ICustomerService> _customerService;
        private ILogger<CustomerController> _logger;
        private List<Customer> _customers;

        [TestInitialize]
        public void Initialize()
        {
            _logger = Mock.Of<ILogger<CustomerController>>();
            _customerService = new Mock<ICustomerService>();
    
            _customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "Nick",
                    LastName = "Jon",
                    Street = "Test Street",
                    City = "Test City",
                    Postcode = "Test Postcode",
                    Country = "Netherlands"
                }
            };
        }


        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsReturned()
        {
            // Arrange
            Customer customer = new Customer
            {
                Id = 1,
                FirstName = "Nick",
                LastName = "Jon",
                Street = "Address line1",
                City = "Test City",
                Postcode = "Test Postcode",
                Country = "Netherlands"
            };

            _customerService.Setup(_ => _.InsertCustomer(customer)).ReturnsAsync(customer);
            var sut = new CustomerController(_logger, _customerService.Object);

            // Act
            var result = (OkObjectResult)await sut.Post(customer);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task ControllerShouldNotPostNewCustomer()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "Nick",
                LastName = "Jon",
                Street = "Address line1",
                City = "Test City",
                Postcode = "Test Postcode",
                Country = "Netherlands"
            };

            _customerService.Setup(_ => _.InsertCustomer(customer)).ReturnsAsync(customer);
            var sut = new CustomerController(_logger, _customerService.Object);

            // Act
            var result = (BadRequestResult)await sut.Post(customer);

            // Assert
            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturn200Status()
        {
            // Arrange
            _customerService.Setup(_ => _.GetCustomers()).ReturnsAsync(_customers);
            var sut = new CustomerController(_logger, _customerService.Object);

            // Act
            var result = (OkObjectResult)await sut.Get();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturn204NoContentStatus()
        {
            // Arrange
            _customerService.Setup(_ => _.GetCustomers()).ReturnsAsync(new List<Customer>());
            var sut = new CustomerController(_logger, _customerService.Object);

            // Act
            var result = (NoContentResult)await sut.Get();

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}
