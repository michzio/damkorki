using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class FeedbacksRepository : Repository<Feedback>, IFeedbacksRepository
	{
		public FeedbacksRepository(DatabaseContext context)
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
