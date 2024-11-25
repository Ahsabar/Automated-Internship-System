using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace api.models
{
    public class Student: AppUser
    {
		public long? Tc { get; set; }
		public int? Year { get; set; }
		public string? Department { get; set; }
		public List<Application> Applications { get; set; } = new List<Application>();
		public StudentInfo StudentInfo { get; set; }
		public Internship Internship { get; set; }
    }
}