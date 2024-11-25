using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Application
    {
        public int Id { get; set; }
		public string StudentId { get; set; }
		public Student? Student { get; set; }
		public int AnnouncementId { get; set; }
		public Announcement? Announcement { get; set; }
		public DateTime AppliedAt { get; set; } = DateTime.Now;
		public DateTime? StatusUpdateDate { get; set; }
		public int Status { get; set; } = 0;
		public bool? IsApprovedByAdmin { get; set; }
		public bool? IsApprovedByCompany { get; set; }
		public bool? IsApprovedBySecretary { get; set; }
		public List<Document> Documents { get; set; } = new List<Document>();
		public Internship Internship { get; set; }
    }
}