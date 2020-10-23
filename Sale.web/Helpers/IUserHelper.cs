using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sale.Common.Enums;
using Sale.web.Data.Entities;
using Sale.web.Models;
using System;
using System.Threading.Tasks;


namespace Sale.web.Helpers
{
    public interface IUserHelper
    {
       Task<User> GetUserAsync(string email);
       Task<IdentityResult> AddUserAsync(User user, string password);
       Task CheckRoleAsync(string roleName);
       Task AddUserToRoleAsync(User User, string roleName);
       Task<bool> IsUserInRoleAsync(User user, string roleName);
       Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<Microsoft.AspNetCore.Identity.SignInResult> ValidatePasswordAsync(User user, string Password);
        Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType);
    }

}
