using Sale.Common.Entities;
using Sale.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sale.Common.Responses
{
   public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Document { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ImageId { get; set; }
        public string ImageFullPath => string.IsNullOrEmpty(ImageId)
               ? $"http://onsaleahmed.somee.com/images/noimage.png"
              : $"http://onsaleahmed.somee.com/{ImageId.Substring(1)}";
        public UserType UserType { get; set; }
        public City City { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
