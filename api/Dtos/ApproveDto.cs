using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ApproveDto
    {
        public bool IsApproved { get; set; }
		public string? FeedBack { get; set; }
    }
}