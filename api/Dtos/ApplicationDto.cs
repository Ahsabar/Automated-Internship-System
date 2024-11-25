using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ApplicationDto
    {
        public int Id { get; set; }
		public string StudentId { get; set; }
		public int AnnouncementId { get; set; }
		public DateTime AppliedAt { get; set; } = DateTime.Now;
		public DateTime? StatusUpdateDate { get; set; }
    }
}