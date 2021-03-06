﻿using Microsoft.AspNetCore.Identity;
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
        Task<User> AddUserAsync(AddUserViewModel model, string imageId, UserType userType);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<User> GetUserAsync(Guid userId);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string Token);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

    }

}
