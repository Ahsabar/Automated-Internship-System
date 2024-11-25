using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.models;

namespace api.Interfaces
{
    public interface IStudentRepository
    {
        Task<StudentInfo> CreateStudentInfoAsync(StudentInfo studentInfo);
		Task<StudentInfo?> UpdateStudentInfoAsync(string id, UpdateStudentInfoDto studentInfoDto);
    }
}