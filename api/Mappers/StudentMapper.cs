using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.models;
using AutoMapper;

namespace api.Mappers
{
    public class StudentMapper: Profile
    {
        public StudentMapper()
		{
			CreateMap<CreateStudentInfoDto, StudentInfo>().ReverseMap();
		}
    }
}