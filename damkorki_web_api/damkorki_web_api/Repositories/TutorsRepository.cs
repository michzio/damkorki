using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class TutorsRepository : Repository<Tutor>, ITutorsRepository
	{
		public TutorsRepository(DatabaseContext context)
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
