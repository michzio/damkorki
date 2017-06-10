using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public interface ILessonOffersRepository : IRepository<LessonOffer>
	{
		// TODO: Custom repository methods

		Task<IEnumerable<LessonOffer>> GetAllEagerlyAsync();
	}
}
