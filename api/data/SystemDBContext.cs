using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.data
{
    public class SystemDBContext: IdentityDbContext<AppUser>
    {
        public SystemDBContext(DbContextOptions<SystemDBContext> dbContextOptions): base(dbContextOptions)
		{
			
		}
		public DbSet<Student> Student { get; set; }
		public DbSet<Company> Company { get; set; }
		public DbSet<Announcement> Announcement { get; set; }
		public DbSet<Application> Application { get; set; }
		public DbSet<Document> Document { get; set; }
		public DbSet<Internship> Internship { get; set; }
		public DbSet<StudentInfo> StudentInfo { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure separate tables for each user type
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Student>().ToTable("Student");
        }

    }
}