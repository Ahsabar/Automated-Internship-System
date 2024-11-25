using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.Interfaces
{
    public interface IInternshipRepository
    {
        Task<Internship> CreateAsync(Internship internshipModel);
    }
}