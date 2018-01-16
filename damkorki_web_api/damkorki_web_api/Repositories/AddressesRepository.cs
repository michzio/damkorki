using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Repositories
{
	public class AddressesRepository : Repository<Address>, IAddressesRepository
	{
		public AddressesRepository(DatabaseContext context)
			: base(context)
		{
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}

		public async Task<Address> GetEagerlyAsync(int addressId) { 

			return await DatabaseContext.Set<Address>()
										.Include(a => a.People)
										.SingleOrDefaultAsync(a => a.AddressId == addressId);
		}
	}
}
