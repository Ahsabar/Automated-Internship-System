using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Interfaces;
using api.models;

namespace api.Repository
{
    public class InternshipRepository: IInternshipRepository
    {
        private readonly SystemDBContext _context;

		public InternshipRepository(SystemDBContext context)
		{
			_context = context;
		}

		public async Task<Internship> CreateAsync(Internship internshipModel)
		{
			await _context.Internship.AddAsync(internshipModel);
            await _context.SaveChangesAsync();
            return internshipModel;
		}
    }
}