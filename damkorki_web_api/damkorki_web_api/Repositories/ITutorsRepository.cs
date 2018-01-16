using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public interface ITutorsRepository : IRepository<Tutor>
	{
		Task<Tutor> GetEagerlyAsync(int tutorId);
		IEnumerable<Tutor> FindEagerly(Expression<Func<Tutor, bool>> predicate);
	}
}
