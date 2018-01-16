using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DamkorkiWebApi.Repositories 
{
    public class ProfilePhotosRepository : Repository<ProfilePhoto>, IProfilePhotosRepository
    {
        public ProfilePhotosRepository(DatabaseContext context) : base(context)
        {
        }

        public DatabaseContext DatabaseContext
        {
            get { return Context as DatabaseContext; }
        }

        public async Task<int> UncheckAllByPersonAsync(Person person) 
        { 
            return await UncheckAllByPersonAsync(person.PersonId); 
        }

        public async Task<int> UncheckAllByPersonAsync(int personId) { 

            // execute SQL command unchecking all photos for given person 
            return await Context.Database.ExecuteSqlCommandAsync(
                        @"UPDATE [ProfilePhoto] SET IsProfilePhoto = 0 WHERE PersonId = @PersonId",
                        new SqlParameter("@PersonId", personId)
                    ); 
        }

        public async Task<int> CheckMainAsync(ProfilePhoto profilePhoto) { 
             
            var rawsUnchecked = 0; 
            var rawsChecked = 0; 

            using(var dbTransaction = await Context.Database.BeginTransactionAsync()) 
            {
                try { 
                    // execute SQL command unchecking all photos for given person 
                    rawsUnchecked = await this.UncheckAllByPersonAsync(profilePhoto.PersonId); 
                    // execute SQL command checking given photo as main profile photo 
                    rawsChecked = await Context.Database.ExecuteSqlCommandAsync(
                                @"UPDATE [ProfilePhoto] SET IsProfilePhoto = 1 WHERE ProfilePhotoId = @ProfilePhotoId",
                                new SqlParameter("@ProfilePhotoId", profilePhoto.ProfilePhotoId)
                                );

                    dbTransaction.Commit(); 
                } 
                catch(Exception) 
                { 
                    dbTransaction.Rollback(); 
                }
            } 

            return rawsChecked; 
        }
    }
}