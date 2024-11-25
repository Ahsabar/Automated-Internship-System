using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Announcement
    {
        public int Id { get; set; }
		public string AnnouncementName { get; set; }
		public string Description { get; set; }
		public byte[]? Image { get; set; } 
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string CompanyId { get; set; }
		public Company? Company { get; set; }
		public string? Status { get; set; } = "Pending";
		public List<Application> Applications { get; set; } = new List<Application>();
    }
}