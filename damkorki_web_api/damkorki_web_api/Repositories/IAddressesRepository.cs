using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public interface IAddressesRepository : IRepository<Address>
	{
		Task<Address> GetEagerlyAsync(int addressId);
	}
}
