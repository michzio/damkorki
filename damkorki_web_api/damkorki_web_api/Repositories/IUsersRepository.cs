using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public interface IUsersRepository : IRepository<ApplicationUser>
	{
		// Custom repository methods

		Task<ApplicationUser> GetEagerlyAsync(string id); 
		Task<ApplicationUser> GetWithPersonProfileAsync(string id);  
		Task<ApplicationUser> GetWithTutorProfileAsync(string id); 
		Task<ApplicationUser> GetWithLearnerProfileAsync(string id);  
	}
}
