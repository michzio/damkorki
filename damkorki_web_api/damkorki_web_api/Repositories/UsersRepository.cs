using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class UsersRepository : Repository<ApplicationUser>, IUsersRepository
	{
		public UsersRepository(DatabaseContext context)
			: base(context)
		{
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}

		// TODO: Implementation of custom repository methods
	}
}
