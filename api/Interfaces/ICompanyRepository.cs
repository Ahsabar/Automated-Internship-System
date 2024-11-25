using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.models;

namespace api.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllAsync(QueryObject queryObject);
    }
}