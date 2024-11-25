using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Mvc;

namespace api.Repository
{
    public class StudentRepository: IStudentRepository
    {
		private readonly SystemDBContext _context;
		
        public StudentRepository(SystemDBContext context)
		{
			_context = context;
		}

		public async Task<StudentInfo> CreateStudentInfoAsync(StudentInfo studentInfo)
		{
			await _context.StudentInfo.AddAsync(studentInfo);
            await _context.SaveChangesAsync();
            return studentInfo;
		}

		public async Task<StudentInfo?> UpdateStudentInfoAsync(string id, UpdateStudentInfoDto studentInfoDto)
		{
			var existingInfo = await _context.StudentInfo.FindAsync(id);

			if(existingInfo == null)
				return null;

			if(!string.IsNullOrWhiteSpace(studentInfoDto.FullName))
				existingInfo.FullName = studentInfoDto.FullName;

			if(!string.IsNullOrWhiteSpace(studentInfoDto.Email))
				existingInfo.Email = studentInfoDto.Email;

			if(studentInfoDto.Degree.HasValue)
				existingInfo.Degree = studentInfoDto.Degree.Value;

			if(!string.IsNullOrWhiteSpace(studentInfoDto.StudentPhone))
				existingInfo.StudentPhone = studentInfoDto.StudentPhone;

			if(!string.IsNullOrWhiteSpace(studentInfoDto.RelativePhone))
				existingInfo.RelativePhone = studentInfoDto.RelativePhone;

			await _context.SaveChangesAsync();

            return existingInfo;
		}
	}
}