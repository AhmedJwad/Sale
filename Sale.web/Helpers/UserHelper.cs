using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sale.web.Data;
using Sale.web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(DataContext context,UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User User, string roleName)
        {
            await _userManager.AddToRoleAsync(User, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if(!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(u => u.City)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
