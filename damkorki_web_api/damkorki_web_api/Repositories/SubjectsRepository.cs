using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class SubjectsRepository : Repository<Subject>, ISubjectsRepository
	{
		public SubjectsRepository(DatabaseContext context)
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
