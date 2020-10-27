using Sale.Common.Entities;
using Sale.Common.Enums;
using Sale.web.Data.Entities;
using Sale.web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper )
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckContriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync( "Ahmed", "Jawad", "Amm380@yahoo.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

        }

        private async Task CheckContriesAsync()
        {
            if(!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name="Iraq",
                    Departments=new List<Department>
                    {
                        new Department
                        {
                            Name="Hilla",
                            Cities=new List<City>
                            {
                                new City
                                {
                                    Name="Hay Al hussein",
                                }
                            }
                        },
                           new Department
                        {
                            Name="baghdad",
                            Cities=new List<City>
                            {
                                new City
                                {
                                    Name="Almonsour",
                                }
                            }
                        },
                            new Department
                        {
                            Name="basra",
                            Cities=new List<City>
                            {
                                new City
                                {
                                    Name="alashar",
                                }
                            }
                        }
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "USA",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "California",
                        Cities = new List<City>
                        {
                            new City { Name = "Los Angeles" },
                            new City { Name = "San Diego" },
                            new City { Name = "San Francisco" }
                        }
                    },
                    new Department
                    {
                        Name = "Illinois",
                        Cities = new List<City>
                        {
                            new City { Name = "Chicago" },
                            new City { Name = "Springfield" }
                        }
                    }
                }
                });
                await _context.SaveChangesAsync();

            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(            
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,                    
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }
            return user;
        }
    }
}
