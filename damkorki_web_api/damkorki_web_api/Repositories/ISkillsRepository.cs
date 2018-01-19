using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public interface ISkillsRepository : IRepository<Skill>
	{
		Task<Skill> GetEagerlyAsync(int skillId); 
	}
}
