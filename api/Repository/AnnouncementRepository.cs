using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Dtos.Announcement;
using api.Helpers;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
	public class AnnouncementRepository : IAnnouncementRepository
	{
		private readonly SystemDBContext _context;

		public AnnouncementRepository(SystemDBContext context)
		{
			_context = context;
		}
		public async Task<Announcement> CreateAsync(Announcement announcementModel)
		{
			await _context.Announcement.AddAsync(announcementModel);
            await _context.SaveChangesAsync();
            return announcementModel;
		}

		public Task<Announcement?> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<Announcement>> GetAllAsync(QueryObject queryObject)
		{
			var announcements = _context.Announcement.AsQueryable();

			if(!string.IsNullOrWhiteSpace(queryObject.AnnouncementName))
			{
				announcements = announcements.Where(a => a.AnnouncementName.Contains(queryObject.AnnouncementName));
			}

			return await announcements.ToListAsync();
		}

		public async Task<List<StudentAnnouncementDto>> GetAllAsyncAsFilteredForStudent(string id, QueryObject queryObject)
		{
			var announcements = _context.Announcement
				.Where(a => a.Status == "Approved" 
					&& a.StartDate <= DateTime.Now 
					&& !_context.Application.Any(app => app.StudentId == id && app.AnnouncementId == a.Id));

			if (!string.IsNullOrWhiteSpace(queryObject.AnnouncementName))
			{
				announcements = announcements.Where(a => a.AnnouncementName.Contains(queryObject.AnnouncementName));
			}

			var announcementDtos = await announcements.Select(a => new StudentAnnouncementDto
			{
				AnnouncementName = a.AnnouncementName,
				Description = a.Description,
				Image = a.Image != null ? $"data:image/png;base64,{Convert.ToBase64String(a.Image)}" : null,
				EndDate = a.EndDate,
				CompanyName = a.Company.Name
			})
			.ToListAsync();

			return announcementDtos;
		}

		public async Task<List<AnnouncementDto>> GetAllAsyncAsFilteredForAdmin()
		{
			var announcements = _context.Announcement
				.Where(a => (a.Status == "Pending" || a.Status == "Edited") && a.EndDate > DateTime.Now);

			var announcementDtos = await announcements.Select(a => new AnnouncementDto
			{
				AnnouncementName = a.AnnouncementName,
				Description = a.Description,
				Image = a.Image != null ? $"data:image/png;base64,{Convert.ToBase64String(a.Image)}" : null,
				StartDate = a.StartDate,
				EndDate = a.EndDate,
				CompanyName = a.Company.Name
			})
			.ToListAsync();

			return announcementDtos;
		}

		public async Task<Announcement?> GetByIdAsync(int id)
		{
			return await _context.Announcement.FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task<Announcement?> UpdateAsync(int id, UpdateAnnouncementDto updateAnnouncementDto)
		{
			var existingAnnouncement = await _context.Announcement.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAnnouncement == null)
                return null;

			if (!string.IsNullOrWhiteSpace(updateAnnouncementDto.AnnouncementName))
        		existingAnnouncement.AnnouncementName = updateAnnouncementDto.AnnouncementName;

			if (!string.IsNullOrWhiteSpace(updateAnnouncementDto.Description))
				existingAnnouncement.Description = updateAnnouncementDto.Description;

			if (updateAnnouncementDto.StartDate.HasValue)
				existingAnnouncement.StartDate = (DateTime)updateAnnouncementDto.StartDate;

			if (updateAnnouncementDto.EndDate.HasValue)
				existingAnnouncement.EndDate = (DateTime)updateAnnouncementDto.EndDate;

			if (updateAnnouncementDto.Image != null && updateAnnouncementDto.Image.Length > 0)
				existingAnnouncement.Image = updateAnnouncementDto.Image;
			
			existingAnnouncement.Status = "Edited";

            await _context.SaveChangesAsync();

            return existingAnnouncement;
		}

		public async Task<string?> ApproveAsync(int id, ApproveDto approveDto)
		{
			var existingAnnouncement = await _context.Announcement.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAnnouncement == null)
                return null;

			if (!approveDto.IsApproved)
			{
				_context.Announcement.Remove(existingAnnouncement);
				await _context.SaveChangesAsync();
				return "Announcement rejected and removed from the system.";
			}
        		
			existingAnnouncement.Status = "Approved";
			await _context.SaveChangesAsync();
			return "Announcement approved";
		}
	}
}