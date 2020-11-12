using Microsoft.EntityFrameworkCore;
using Sale.Common.Entities;
using Sale.Common.Enums;
using Sale.web.Data.Entities;
using Sale.web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Ahmed", "Almershady", "Amm380@yahoo.com", "322 311 4620", "Babil", UserType.Admin);
            await CheckCategoriesAsync();
            await CheckProductsAsync();
        }

        private async Task CheckProductsAsync()
        {
           if(!_context.Products.Any())
            {
                User user = await _userHelper.GetUserAsync("Amm380@yahoo.com");
                Category Laptop = await _context.Categories.FirstOrDefaultAsync(C => C.Name == "Laptop");
                Category Mobile = await _context.Categories.FirstOrDefaultAsync(C => C.Name == "Mobile");
                Category accesories = await _context.Categories.FirstOrDefaultAsync(C => C.Name == "accesories");
                String description= "Lorem ipsum dolor sit amet," +
                    " consectetur adipiscing elit. Moors pregnant," +
                    " now running or sad, outdoor volleyball nibh we do not need pillow lorem vegetarian " +
                    "recipes. Microwave undertakes soccer players, but the element Tech yet." +
                    " The temperature dolor who is pregnant sit element zero." +
                    " Class began Employment twist by our marriage, per himenaeos." +
                    " Integer invest only Pakistan, but the makeup pull at it. " +
                    "Sometimes they malesuada hunger and at the first taste." +
                    " Members of nutrition care time. Clinical weekend," +
                    " chili carrots invest pregnant, is pregnant with laughter," +
                    " a ultricies even more great pain. Fusce egestas venenatis velit," +
                    " a ultrices purus aliquet sed." +
                    " Vulputate soccer graph carrots until soft. Present the weekend temperature. Always," +
                    " also, and, for that was at pulvinar ligula. However, there was a lot of sauce," +
                    " but across the country. Manufacturing photography sad enhanced." +
                    " In lorem sapien now in the manufacturing of bananas or a lion..";
                await AddProductAsync(Laptop, description, "Acer", 25000M, new String[]
                {
                    "Bulldog1", "Bulldog2", "Bulldog3", "Bulldog4"
                },user);
                await AddProductAsync(Mobile, description, "Acer", 25000M, new String[]
               {
                    "Bulldog1", "Bulldog2", "Bulldog3", "Bulldog4"
               }, user);
                await AddProductAsync(accesories, description, "Acer", 25000M, new String[]
             {
                    "Bulldog1", "Bulldog2", "Bulldog3", "Bulldog4"
             }, user);
                await _context.SaveChangesAsync();

            }
        }

        private async Task AddProductAsync(Category category, string description, string name, decimal price, string[] images, User user)
        {
            Product product = new Product
            {
                Category = category,
                Description = description,
                Name = name,
                Price = price,
               ProductImages = new List<ProductImage>(),
              Qualifications = GetRandomQualifications(description, user),
              IsActive=true,
              IsStarred=false,
            };
            //foreach (var image in images)
            //{
            //    string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images", $"{image}.png");
            //    Guid imageId = await _blobHelper.UploadBlobAsync(path, "products");
            //    product.ProductImages.Add(new ProductImage { ImageId = imageId });
            //}
            _context.Products.Add(product);
        }

        private ICollection<Qualification> GetRandomQualifications(string description, User user)
        {
            List<Qualification> qualifications = new List<Qualification>();
            for (int i = 0; i < 10; i++)
            {
                qualifications.Add(new Qualification
                {
                    Date=DateTime.UtcNow,
                    Remarks=description,
                    Score=_random.Next(1,5),
                    User=user,

                });
            }
            return qualifications;
        }

        private async Task CheckCategoriesAsync()
        {
            if(!_context.Categories.Any())
            {
                await AddCategoryAsync("Laptop");
                await AddCategoryAsync("Mobile");
                await AddCategoryAsync("accesories");
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddCategoryAsync(string name)
        {           
                //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images", $"{name}.png");
                //Guid imageId = await _blobHelper.UploadBlobAsync(path, "categories");     
               _context.Categories.Add(new Category { Name = name });

        }

        private async Task<User> CheckUserAsync(string firstname,string lastname, string email, string phonenumber, string address, UserType usertype)
        {
            User user = await _userHelper.GetUserAsync(email);
            if(user==null)
            {
                user = new User
                {
                    FirstName = firstname,
                    LastName = lastname,
                    PhoneNumber = phonenumber,
                    Address = address,
                    Email = email,
                    UserName = email,
                    UserType = usertype,
                    City = _context.Cities.FirstOrDefault(),
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, usertype.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
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
                            Name="Baghdad",
                            Cities=new List<City>
                            {
                                new City{Name="AL mounsor"},
                                new City{Name="Alkarada"},
                                new City{Name="wathaq street"}
                            }
                        },
                        new Department
                        {
                            Name="Babylon",
                            Cities=new List<City>
                            {
                                new City{Name="hay alhussein"},
                                new City{Name="alkarama"},
                                new City{Name="al jameaee"},
                            }
                        }                    
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name="Usa",
                    Departments=new List<Department>
                    {
                       new Department
                       {
                           Name="California",
                           Cities=new List<City>
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

