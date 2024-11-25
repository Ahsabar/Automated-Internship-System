using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class StudentInfo
    {
		[Key, ForeignKey("Student")]
        public string StudentId { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public int Degree { get; set; }
		public int StudentNo { get; set; }
		public long TcNo { get; set; }
		public string StudentPhone { get; set; }
		public string RelativePhone { get; set; }
		public Student? Student { get; set; }
    }
}