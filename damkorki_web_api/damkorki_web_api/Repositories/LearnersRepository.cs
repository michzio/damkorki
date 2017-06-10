using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class LearnersRepository : Repository<Learner>, ILearnersRepository
	{
		public LearnersRepository(DatabaseContext context)
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
