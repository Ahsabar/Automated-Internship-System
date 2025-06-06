using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class AnnouncementDto
    {
		public int Id { get; set; }
		public string AnnouncementName { get; set; }
		public string Description { get; set; }
		public string? Image { get; set; } 
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string CompanyId { get; set; }
		public string? Status { get; set; } = "Pending";
		public string CompanyName { get; set; }
    }
}