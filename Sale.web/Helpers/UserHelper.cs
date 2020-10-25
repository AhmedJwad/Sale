using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sale.Common.Enums;
using Sale.web.Data;
using Sale.web.Data.Entities;
using Sale.web.Models;
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
        private readonly SignInManager<User> _signInManager;

        public UserHelper(DataContext context,UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType)
        {
            User user = new User
            {
                Address=model.Address,
                FirstName=model.FirstName,
                LastName=model.LastName,
                ImageId=imageId,
                Email=model.Username,
                PhoneNumber=model.PhoneNumber,
                City=await _context.Cities.FindAsync(model.CityId),
                UserType= userType,
                UserName=model.Username,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if(result!=IdentityResult.Success)
            {
                return null;
            }

            User newuser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newuser, user.UserType.ToString());
            return newuser;
        }

        public async Task AddUserToRoleAsync(User User, string roleName)
        {
            await _userManager.AddToRoleAsync(User, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
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

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users
                 .Include(u => u.City)
                 .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync
                (
                  model.Username,
                  model.Password,
                  model.RememberMe,
                  false
                );
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string Password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, Password, false);
        }
    }
}
