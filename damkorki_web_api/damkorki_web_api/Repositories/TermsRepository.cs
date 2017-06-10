using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class TermsRepository : Repository<Term>, ITermsRepository
	{
		public TermsRepository(DatabaseContext context)
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
