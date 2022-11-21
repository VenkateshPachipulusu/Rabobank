using System.Collections.Generic;
using Rabobank.TechnicalTest.GCOB.Dtos;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Repositories
{
    public interface IAddressRepository
    {
        Task<int> GenerateIdentityAsync();
        Task InsertAsync(AddressDto address);
        Task<AddressDto> GetAsync(int identity);
        Task UpdateAsync(AddressDto address);
        Task<IEnumerable<AddressDto>> GetAllAsync();
    }
}
