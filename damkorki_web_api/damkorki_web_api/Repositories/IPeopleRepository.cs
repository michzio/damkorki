using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public interface IPeopleRepository : IRepository<Person>
	{
		IEnumerable<Person> GetFemales(Expression<Func<Person, bool>> predicate);
		IEnumerable<Person> GetMales(Expression<Func<Person, bool>> predicate);
		Task<IEnumerable<Person>> GetAllEagerlyAsync(); 

		Task<Person> GetEagerlyAsync(int personId); 
	}
}
