
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
    public interface ITutorsSkillsRepository : IRepository<TutorSkill>
    {
        Task<TutorSkill> GetAsync((int, int) tutorIdSkillId); 
    }
}