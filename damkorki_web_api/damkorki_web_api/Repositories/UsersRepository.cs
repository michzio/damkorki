using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

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

		// Implementation of custom repository methods

		public async Task<ApplicationUser> GetEagerlyAsync(string id) { 

			return await DatabaseContext.Set<ApplicationUser>()
										.Include(au => au.Person)
										.SingleOrDefaultAsync(au => au.Id == id); 
		}   

		public async Task<ApplicationUser> GetWithPersonProfileAsync(string id) { 

			return await DatabaseContext.Set<ApplicationUser>()
										.Include(au => au.Person)
											.ThenInclude(p => p.Address)
										.Include(au => au.Person)
											.ThenInclude(p => p.ProfilePhotos)
										.SingleOrDefaultAsync(au => au.Id == id); 
		}

		public async Task<ApplicationUser> GetWithTutorProfileAsync(string id) { 

			return await DatabaseContext.Set<ApplicationUser>() 
										.Include(au => au.Person)
											.ThenInclude(p => p.Tutor)
										.SingleOrDefaultAsync(au => au.Id == id);
		}

		public async Task<ApplicationUser> GetWithLearnerProfileAsync(string id) { 

			return await DatabaseContext.Set<ApplicationUser>() 
										.Include(au => au.Person)
											.ThenInclude(p => p.Learner)
										.SingleOrDefaultAsync(au => au.Id == id);
		}
	}
}
