using System;
using System.Collections.Generic;

namespace WebApplication11.Models
{
    public partial class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string MiddleName { get; set; }
    }
}
