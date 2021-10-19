﻿using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Core.Models;
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
            //ToDo Make more MAPPINGS

            CreateMap<Work, AuthorWorkDto>().ReverseMap();
            //.ForMember....BOOKING
            CreateMap<Work, WorkPutDto>().ReverseMap();

            CreateMap<Work, WorkDto>().ReverseMap();
            CreateMap<Work, WorkCreateDto>().ReverseMap();

            CreateMap<AuthorViewModel, AuthorDto>().ReverseMap();
        }

    }
}
