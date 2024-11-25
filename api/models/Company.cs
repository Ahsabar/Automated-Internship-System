using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.models
{
    public class Company: AppUser
    {
		public string Name { get; set; }
		public string? StatusByDIC { get; set; }
		public string Address { get; set; }
		public List<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}