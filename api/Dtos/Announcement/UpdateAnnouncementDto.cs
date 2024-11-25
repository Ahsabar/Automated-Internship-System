using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class UpdateAnnouncementDto
    {
		[MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string? AnnouncementName { get; set; }

		[MinLength(5, ErrorMessage = "Description must be at least 5 characters")]
        [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
		public string? Description { get; set; }
		public byte[]? Image { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
    }
}