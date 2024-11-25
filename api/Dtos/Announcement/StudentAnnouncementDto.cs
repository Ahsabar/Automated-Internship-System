using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Announcement
{
    public class StudentAnnouncementDto
    {
		public string AnnouncementName { get; set; }
		public string Description { get; set; }
		public string? Image { get; set; } 
		public DateTime EndDate { get; set; }
		public string CompanyName { get; set; }
    }
}