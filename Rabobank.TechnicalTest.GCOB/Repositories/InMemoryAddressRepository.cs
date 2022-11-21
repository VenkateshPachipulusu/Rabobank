using System;
using Rabobank.TechnicalTest.GCOB.Dtos;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rabobank.TechnicalTest.GCOB.Repositories
{
    public class InMemoryAddressRepository : IAddressRepository
    {
        private ConcurrentDictionary<int, AddressDto> Addresses { get; } = new ConcurrentDictionary<int, AddressDto>();
        private ILogger _logger;

        public InMemoryAddressRepository(ILogger logger)
        {
            _logger = logger;
         //   Addresses.TryAdd(1, new AddressDto {Id = 1,Street = "James Street", City = "London", Postcode = "X1A 6TH", CountryId = 1});
        }

        public Task<AddressDto> GetAsync(int identity)
        {
            _logger.LogDebug($"FindMany Addresses with identity {identity}");

            if (!Addresses.ContainsKey(identity)) throw new Exception(identity.ToString());
            _logger.LogDebug($"Found Address with identity {identity}");
            return Task.FromResult(Addresses[identity]);
        }

        Task<int> IAddressRepository.GenerateIdentityAsync()
        {
            _logger.LogDebug("Generating Address identity");
            return Task.Run(() =>
            {
                if (Addresses.Count == 0) return 1;

                var x = Addresses.Values.Max(c => c.Id);
                return ++x;
            });
        }

        Task<AddressDto> IAddressRepository.GetAsync(int identity)
        {
            _logger.LogDebug($"FindMany Addresses with identity {identity}");

            if (!Addresses.ContainsKey(identity)) throw new Exception(identity.ToString());
            _logger.LogDebug($"Found Address with identity {identity}");
            return Task.FromResult(Addresses[identity]);
        }

        Task IAddressRepository.InsertAsync(AddressDto address)
        {
            if (Addresses.ContainsKey(address.Id))
            {
                throw new Exception(
                    $"Cannot insert customer with identity '{address.Id}' " +
                    "as it already exists in the collection");
            }

            Addresses.TryAdd(address.Id, address);
            _logger.LogDebug($"New address inserted [ID:{address.Id}]. " +
                             $"There are now {Addresses.Count} legal entities in the store.");
            return Task.FromResult(address);
        }

        Task IAddressRepository.UpdateAsync(AddressDto address)
        {
            if (!Addresses.ContainsKey(address.Id))
            {
                throw new Exception(
                    $"Cannot update address with identity '{address.Id}' " +
                    "as it doesn't exist");
            }

            Addresses[address.Id] = address;
            _logger.LogDebug($"New address updated [ID:{address.Id}].");

            return Task.FromResult(address);
        }

        public Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            return Task.FromResult(Addresses.Select(x => x.Value));
        }
    }
}
