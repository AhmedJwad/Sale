using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sale.Common.Entities;
using Sale.Common.Enums;
using Sale.Common.Request;
using Sale.Common.Responses;
using Sale.web.Data;
using Sale.web.Data.Entities;
using Sale.web.Helpers;
using Sale.web.Models;

namespace Sale.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public AccountController(IUserHelper userHelper, IConfiguration configuration,
            IMailHelper mailHelper, DataContext context, IImageHelper imageHelper )
        {
           _userHelper = userHelper;
           _configuration = configuration;
           _mailHelper = mailHelper;
           _context = context;
            _imageHelper = imageHelper;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new Response
                { 
                    IsSuccess=false,
                    Message="Bad request",
                    Result=ModelState,                
                });
            }
            User user = await _userHelper.GetUserAsync(request.Email);
            if(user!=null)
            {
                return BadRequest(new Response
                {
                    IsSuccess=false,
                    Message= "Error003",
                });
            }
            City city = await _context.Cities.FindAsync(request.CityId);
            if(city==null)
            {
                return BadRequest(new Response
                {
                    IsSuccess=false,
                    Message="Error004",
                });
            }
            string imageId = string.Empty;
            if(request.ImageArray!=null)
            {
                imageId =  _imageHelper.UploadImage(request.ImageArray, "users");
            }
            user = new User
            {
                FirstName=request.FirstName,
                LastName=request.LastName,
                Address=request.Address,
                City=city,
                ImageId=imageId,
                Email=request.Email,
                UserName=request.Email,
                PhoneNumber=request.Phone,
               UserType=UserType.User,
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            if(result!=IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }
            User userNew = await _userHelper.GetUserAsync(request.Email);
            await _userHelper.AddUserToRoleAsync(userNew, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Email, "Email Confirmation", $"<h1>Email Confirmation</h1>" +
                $"To confirm your email please click on the link<p><a href = \"{tokenLink}\">Confirm Email</a></p>");

            return Ok(new Response { IsSuccess = true });
        }

        [HttpPost]
       [Route("CreateToken")]
       public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model )
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddYears(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost]
        //[Route("GetUserByEmail")]
        //public async Task<IActionResult> GetUserByEmail([FromBody] emailrequest request)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    User user = await _userHelper.GetUserAsync(request.Email);
        //    if(user==null)
        //    {
        //        return NotFound("Error001");
        //    }
        //    return Ok(user);
        //}
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if(user==null)
            {
                return NotFound("Error001");
            }
            return Ok(user);
        }
        [HttpPost]
        [Route("RecoverPasswordApp")]
        public async Task<IActionResult> RecoverPasswordApp([FromBody] emailrequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error001"
                });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Password Recover", $"<h1>Password Recover</h1>" +
                $"Click on the following link to change your password:<p>" +
                $"<a href = \"{link}\">Change Password</a></p>");

            return Ok(new Response { IsSuccess = true });
        }
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if(user==null)
            {
                return NotFound("Error001");
            }
            City city = await _context.Cities.FindAsync(request.CityId);
            if(city==null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error004",
                });
            }

            string imageId = user.ImageId;
            if(request.ImageArray!=null)
            {
                imageId = _imageHelper.UploadImage(request.ImageArray, "users");
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.ImageId = imageId;
            user.City = city;
            user.Address = request.Address;
            IdentityResult result = await _userHelper.UpdateUserAsync(user);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }
            User updateuser = await _userHelper.GetUserAsync(email);
            return Ok(updateuser);
        }

        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePasswordApp")]
        public async Task<IActionResult> ChangePasswordApp(ChangePasswordRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState,
                });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await  _userHelper.GetUserAsync(email);
            if(user==null)
            {
                return NotFound("Error001");
            }
            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword,
                request.NewPassword);
            if(!result.Succeeded)
            {
                var message = result.Errors.FirstOrDefault().Description;
                return BadRequest(new Response
                {
                    IsSuccess=false,
                    Message="Error005",
                });
            }
            return Ok(new Response { IsSuccess = true });
        }
    }
}

