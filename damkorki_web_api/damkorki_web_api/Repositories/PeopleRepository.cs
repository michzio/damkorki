using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using DamkorkiWebApi.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Repositories
{
	public class PeopleRepository : Repository<Person>, IPeopleRepository
	{
		public PeopleRepository(DatabaseContext context)
			: base(context)
		{
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}

		IEnumerable<Person> IPeopleRepository.GetFemales(Expression<Func<Person, bool>> predicate)
		{
			return DatabaseContext.People.Where(p => p.Gender == Person.GenderType.Female);
		}

		public IEnumerable<Person> GetMales(Expression<Func<Person, bool>> predicate)
		{
			return DatabaseContext.People.Where(p => p.Gender == Person.GenderType.Male);
		}

		public async Task<IEnumerable<Person>> GetAllEagerlyAsync() { 

			return await DatabaseContext.Set<Person>()
										.Include(p => p.ApplicationUser)
										.ToListAsync(); 
		}
	}
}
