using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

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

		public async Task<Tutor> GetEagerlyAsync(int tutorId) { 

			return await DatabaseContext.Set<Tutor>()
										.Include(t => t.Person)
										.Include(t => t.LessonOffers)
										.Include(t => t.Reservations)
										.Include(t => t.Experiences)
										.Include(t => t.TutorSkills)
											.ThenInclude(ts => ts.Skill)
										.SingleOrDefaultAsync(t => t.TutorId == tutorId); 
		}

		public IEnumerable<Tutor> FindEagerly(Expression<Func<Tutor, bool>> predicate) { 

			return DatabaseContext.Set<Tutor>()
								  .Where(predicate)
								  .Include(t => t.Person)
								  .Include(t => t.LessonOffers)
						     	  .Include(t => t.Reservations)
								  .Include(t => t.Experiences)
								  .Include(t => t.TutorSkills)
								  	.ThenInclude(ts => ts.Skill);
		}
	}
}
