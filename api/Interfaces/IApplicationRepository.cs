using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Helpers;
using api.models;

namespace api.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application> CreateAsync(Application applicationModel);
		Task<AdminApplicationDto?> GetByIdAsync(int id);
		Task<List<AdminApplicationDto>> GetAllAsyncForAdmin(QueryObject queryObject);
		Task<List<AdminApplicationDto>> GetAllAsyncForCompany(QueryObject queryObject);
		Task<List<AdminApplicationDto>> GetAllAsyncForSecretary(QueryObject queryObject);
		Task<(Application?, string)?> ApproveApplicationAsyncForCompany(int id, ApproveDto approveDto);
		Task<(Application?, string)?> ApproveApplicationAsyncForAdmin(int id, ApproveDto approveDto);
		Task<(Application?, string)?> ApproveApplicationAsyncForSecretary(int id, ApproveDto approveDto);
    }
}