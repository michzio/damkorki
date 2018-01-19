
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Repositories 
{
    public class TutorsSkillsRepository : Repository<TutorSkill>, ITutorsSkillsRepository
    {
        public TutorsSkillsRepository(DbContext context) : base(context)
        {
        }

        public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}

        public async Task<TutorSkill> GetAsync( (int, int) tutorIdSkillId) { 

            return await DatabaseContext.Set<TutorSkill>()
                                        .SingleOrDefaultAsync(ts => ts.TutorId == tutorIdSkillId.Item1 && ts.SkillId == tutorIdSkillId.Item2); 
        }
    }
}