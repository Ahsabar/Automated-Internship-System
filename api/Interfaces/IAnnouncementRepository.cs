using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Announcement;
using api.Helpers;
using api.models;

namespace api.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<List<Announcement>> GetAllAsync(QueryObject queryObject);
		Task<List<StudentAnnouncementDto>> GetAllAsyncAsFilteredForStudent(string Id, QueryObject queryObject);
		Task<List<AnnouncementDto>> GetAllAsyncAsFilteredForAdmin();
        Task<Announcement?> GetByIdAsync(int id);
        Task<Announcement> CreateAsync(Announcement announcementModel);
        Task<Announcement?> UpdateAsync(int id, UpdateAnnouncementDto updateAnnouncementDto);
		Task<string?> ApproveAsync(int id, ApproveDto approveDto);
        Task<Announcement?> DeleteAsync(int id);
    }
}