using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.models
{
    public class AppUser: IdentityUser
    {
		public string? FullName { get; set; }
		public string? UserType { get; set; }
    }
}