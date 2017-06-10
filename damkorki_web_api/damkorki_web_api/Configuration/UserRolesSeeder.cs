using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DamkorkiWebApi.Configuration {

    public class UserRolesSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UserRolesSeeder(RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            await CreateRoleAsync("Admin"); 
            await CreateRoleAsync("Moderator");
            await CreateRoleAsync("User");
        }

        public async Task CreateRoleAsync(string name) { 

            if( !(await _roleManager.RoleExistsAsync(name)) )
                await _roleManager.CreateAsync(new IdentityRole { Name = name});

        }

    }
}