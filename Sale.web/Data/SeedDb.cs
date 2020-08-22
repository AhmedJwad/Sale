using Sale.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckContriesAsync();
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
    }
}
