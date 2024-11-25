using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Document
    {
        public int Id { get; set; }
		public int ApplicationId { get; set; }
		public Application? Application { get; set; }
		public string? UserId { get; set; }
		public string Name { get; set; }
		public string FileType { get; set; }
		public byte[] Data { get; set; }
		public string? Status { get; set; }
    }
}