using System;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories 
{
    public interface IProfilePhotosRepository : IRepository<ProfilePhoto>
    {
        Task<int> UncheckAllByPersonAsync(int personId);
        Task<int> UncheckAllByPersonAsync(Person person); 
        Task<int> CheckMainAsync(ProfilePhoto profilePhoto); 

    }
}