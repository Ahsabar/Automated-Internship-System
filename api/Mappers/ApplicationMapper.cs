using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.models;
using AutoMapper;

namespace api.Mappers
{
    public class ApplicationMapper: Profile
    {
        public ApplicationMapper()
		{
			CreateMap<ApplicationDto, Application>().ReverseMap();
		}
    }
}