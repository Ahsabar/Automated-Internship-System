using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Internship
    {
		[Key, ForeignKey("Application")]
        public int Id { get; set; }

		[ForeignKey("Student")]
		public string StudentId { get; set; }
		public string StudentName { get; set; }
		public string Status { get; set; } = "Started";
		public string IsApproved { get; set; }
		public string Score { get; set; }
		public Student? Student { get; set; }
		public Application Application { get; set; }
    }
}