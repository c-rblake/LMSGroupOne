﻿using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<Author, WorkAuthorDto>().ReverseMap();
            CreateMap<Author, AuthorPatchDto>().ReverseMap();
            //ToDo Make more MAPPINGS

            CreateMap<Work, AuthorWorkDto>().ReverseMap();
            CreateMap<Work, WorkPatchDto>().ReverseMap();
            //.ForMember....BOOKING
            CreateMap<Work, WorkPutDto>().ReverseMap();

            CreateMap<Work, WorkDto>().ReverseMap();
            CreateMap<Work, WorkCreateDto>().ReverseMap();


        }

    }
}
