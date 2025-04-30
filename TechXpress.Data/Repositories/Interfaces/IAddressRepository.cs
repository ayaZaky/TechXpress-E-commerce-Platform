using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;

namespace TechXpress.Data.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> CreateAddressAsync(Address address);
        Task<Address> GetAddressAsync(int id);
        Task<IEnumerable<Address>> GetUserAddressesAsync(string userId);
        Task<Address> UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(int id);
    }

}
