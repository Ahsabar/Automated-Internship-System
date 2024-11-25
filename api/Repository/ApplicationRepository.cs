using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
	public class ApplicationRepository : IApplicationRepository
	{
		private readonly SystemDBContext _context;
		public ApplicationRepository(SystemDBContext context)
		{
			_context = context;
		}
		public async Task<Application> CreateAsync(Application applicationModel)
		{
			await _context.Application.AddAsync(applicationModel);
            await _context.SaveChangesAsync();
            return applicationModel;
		}

		public async Task<List<AdminApplicationDto>> GetAllAsyncForAdmin(QueryObject queryObject)
		{
			var applications = _context.Application
				.Where(a => a.IsApprovedByCompany == true && a.IsApprovedByAdmin == null);

			if(!string.IsNullOrWhiteSpace(queryObject.AnnouncementName))
			{
				applications = applications.Where(a => a.Announcement.AnnouncementName.Contains(queryObject.AnnouncementName));
			}

			var applicationDtos = await applications.Select(a => new AdminApplicationDto
			{
				CompanyName = a.Announcement.Company.Name,
				StudentName = a.Student.FullName,
			})
			.ToListAsync();

			return applicationDtos;
		}

		public async Task<List<AdminApplicationDto>> GetAllAsyncForCompany(QueryObject queryObject)
		{
			var applications = _context.Application
				.Where(a => a.IsApprovedByCompany == null);

			if(!string.IsNullOrWhiteSpace(queryObject.AnnouncementName))
			{
				applications = applications.Where(a => a.Announcement.AnnouncementName.Contains(queryObject.AnnouncementName));
			}

			var applicationDtos = await applications.Select(a => new AdminApplicationDto
			{
				CompanyName = a.Announcement.Company.Name,
				StudentName = a.Student.FullName,
			})
			.ToListAsync();

			return applicationDtos;
		}

		public async Task<List<AdminApplicationDto>> GetAllAsyncForSecretary(QueryObject queryObject)
		{
			var applications = _context.Application
				.Where(a => a.IsApprovedBySecretary == null && a.IsApprovedByAdmin == true);

			if(!string.IsNullOrWhiteSpace(queryObject.AnnouncementName))
			{
				applications = applications.Where(a => a.Announcement.AnnouncementName.Contains(queryObject.AnnouncementName));
			}

			var applicationDtos = await applications.Select(a => new AdminApplicationDto
			{
				CompanyName = a.Announcement.Company.Name,
				StudentName = a.Student.FullName,
			})
			.ToListAsync();

			return applicationDtos;
		}

		public async Task<AdminApplicationDto?> GetByIdAsync(int id)
		{
			var applicationDto = await _context.Application
				.Where(application => application.Id == id)
				.Select(a => new AdminApplicationDto
				{
					CompanyName = a.Announcement.Company.Name,
					StudentName = a.Student.FullName,
				})
				.FirstOrDefaultAsync();

			return applicationDto;
		}

		public async Task<(Application?, string)?> ApproveApplicationAsyncForCompany(int id, ApproveDto approveDto)
		{
			var existingApplication = await _context.Application.FirstOrDefaultAsync(x => x.Id == id);

            if (existingApplication == null)
                return null;

			existingApplication.StatusUpdateDate = DateTime.Now;

			if (!approveDto.IsApproved)
			{
				existingApplication.IsApprovedByCompany = false;
				existingApplication.Status = 4;
				await _context.SaveChangesAsync();
				return (existingApplication, "Application rejected.");
			}
        		
			existingApplication.IsApprovedByCompany = true;
			existingApplication.Status = 1;
			await _context.SaveChangesAsync();
			return (existingApplication, "Application approved.");
		}

		public async Task<(Application?, string)?> ApproveApplicationAsyncForAdmin(int id, ApproveDto approveDto)
		{
			// admin reddedince company e geri dönmesi lazım
			var existingApplication = await _context.Application.FirstOrDefaultAsync(x => x.Id == id);

            if (existingApplication == null)
                return null;

			existingApplication.StatusUpdateDate = DateTime.Now;

			if (!approveDto.IsApproved)
			{
				existingApplication.IsApprovedByAdmin = false;
				existingApplication.Status = 4;
				await _context.SaveChangesAsync();
				return (existingApplication, "Application rejected");
			}
        		
			existingApplication.IsApprovedByAdmin = true;
			existingApplication.Status = 2;
			await _context.SaveChangesAsync();
			return (existingApplication, "Application approved");
		}

		public async Task<(Application?, string)?> ApproveApplicationAsyncForSecretary(int id, ApproveDto approveDto)
		{
			var existingApplication = await _context.Application.FirstOrDefaultAsync(x => x.Id == id);

            if (existingApplication == null)
                return null;

			existingApplication.StatusUpdateDate = DateTime.Now;

			if (!approveDto.IsApproved)
			{
				existingApplication.IsApprovedBySecretary = false;
				existingApplication.Status = 4;
				await _context.SaveChangesAsync();
				return (existingApplication, "Application is sent back to admin");
			}
        		
			existingApplication.IsApprovedBySecretary = true;
			existingApplication.Status = 3;
			await _context.SaveChangesAsync();
			return (existingApplication, "Employment certificate sent");
		}
	}
}