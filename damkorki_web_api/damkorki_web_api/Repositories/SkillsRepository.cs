using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Repositories
{
	public class SkillsRepository : Repository<Skill>, ISkillsRepository
	{
		public SkillsRepository(DatabaseContext context)
			: base(context)
		{
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}

		public async Task<Skill> GetEagerlyAsync(int skillId) { 
			
			return await DatabaseContext.Set<Skill>()
										.Include(s => s.TutorSkills)
											.ThenInclude(ts => ts.Tutor)
										.SingleOrDefaultAsync(s => s.SkillId == skillId);
		}
	}
}
