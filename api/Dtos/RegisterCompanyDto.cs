using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class RegisterCompanyDto: RegisterDto
    {
		[Required]
        public string? Name { get; set; }

		[Required]
		public string? FullName { get; set; }

		[Required]
		public string? Address { get; set; }
    }
}