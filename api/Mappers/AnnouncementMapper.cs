using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.models;
using AutoMapper;

namespace api.Mappers
{
    public class AnnouncementMapper: Profile
    {
        public AnnouncementMapper()
		{
			CreateMap<AnnouncementDto, Announcement>().ReverseMap();
			CreateMap<CreateAnnouncementDto, Announcement>().ReverseMap();
		}
    }
}