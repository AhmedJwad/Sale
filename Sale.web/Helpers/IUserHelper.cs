using Microsoft.AspNetCore.Identity;
using Sale.web.Data.Entities;
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

      }

    }
