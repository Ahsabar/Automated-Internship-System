using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Dtos
{
    public class CreateStudentInfoDto
    {
		[Required]
        public string? FullName { get; set; }

		[Required]
		public string? Email { get; set; }

		[Required]
		public int Degree { get; set; }

		[Required]
		public int StudentNo { get; set; }

		[Required]
		public long TcNo { get; set; }

		[Required]
		public string? StudentPhone { get; set; }

		[Required]
		public string? RelativePhone { get; set; }
    }
}