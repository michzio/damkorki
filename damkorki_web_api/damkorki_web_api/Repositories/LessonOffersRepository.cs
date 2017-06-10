using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Repositories
{
	public class LessonOffersRepository : Repository<LessonOffer>, ILessonOffersRepository
	{
		public LessonOffersRepository(DatabaseContext context)
			: base(context)
		{
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}

		// TODO: Implementation of custom repository methods

		public async Task<IEnumerable<LessonOffer>> GetAllEagerlyAsync() { 


			return  await DatabaseContext.Set<LessonOffer>()
										 .Include(lo => lo.Subject)
										 .Include(lo => lo.Tutor)
										 .ToListAsync();
		}

	}
}