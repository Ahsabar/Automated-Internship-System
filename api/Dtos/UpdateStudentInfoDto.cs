using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class UpdateStudentInfoDto
    {
        public string? FullName { get; set; }
		public string? Email { get; set; }
		public int? Degree { get; set; }
		public string? StudentPhone { get; set; }
		public string? RelativePhone { get; set; }
    }
}