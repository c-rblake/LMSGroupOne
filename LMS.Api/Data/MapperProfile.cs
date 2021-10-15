using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Helpers;
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
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge(src.DateOfDeath))) 
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();
           
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
