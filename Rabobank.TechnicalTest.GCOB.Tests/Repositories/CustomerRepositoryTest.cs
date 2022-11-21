using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        private MockRepository _mockRepository;

        private Mock<ILogger> _mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            this._mockRepository = new MockRepository(MockBehavior.Loose);

            this._mockLogger = this._mockRepository.Create<ILogger>();
        }

        private InMemoryCustomerRepository CreateInMemoryCustomerRepository()
        {
            return new InMemoryCustomerRepository(this._mockLogger.Object);
        }

        [TestMethod]
        public async Task GenerateIdentityAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var inMemoryCustomerRepository = this.CreateInMemoryCustomerRepository();

            // Act
            var result = await inMemoryCustomerRepository.GenerateIdentityAsync();

            // Assert
            Assert.IsTrue(result > 0);
            this._mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndIGetTheCustomerFromTheDB_ThenTheCustomerIsRetrieved()
        {
            // Arrange
            var inMemoryCustomerRepository = this.CreateInMemoryCustomerRepository();
            CustomerDto customer = new CustomerDto()
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                AddressId = 1
            };

            // Act
            var customerDto = await inMemoryCustomerRepository.InsertAsync(customer);

            // Assert
            Assert.IsTrue(customerDto.Id > 0);
            this._mockRepository.VerifyAll();
        }
    }
}
