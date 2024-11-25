using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Helpers;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly SystemDBContext _context;

		public CompanyRepository(SystemDBContext context)
		{
			_context = context;
		}

		public async Task<List<Company>> GetAllAsync(QueryObject queryObject)
		{
			var companies = _context.Company.AsQueryable();

			if(!string.IsNullOrWhiteSpace(queryObject.CompanyName))
			{
				companies = companies.Where(a => a.Name.Contains(queryObject.CompanyName));
			}

			return await companies.ToListAsync();
		}
	}
}