using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class RegisterDto
    {
		[Required]
        public string? Username { get; set; }

		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
		public string? FullName { get; set; }

		[Required]
		public string? UserType { get; set; }
    }
}