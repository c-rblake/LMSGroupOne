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
            //Author Mappings
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge(src.DateOfDeath))) 
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();
           
            CreateMap<Author, WorkAuthorDto>()
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge(src.DateOfDeath)))
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<Author, AuthorPatchDto>().ReverseMap();


            //WORK MAPPINGS
            CreateMap<Work, AuthorWorkDto>().ReverseMap();
            CreateMap<Work, WorkPatchDto>().ReverseMap();
            CreateMap<Work, WorkPutDto>().ReverseMap();
            CreateMap<Work, WorkDto>().ReverseMap();
            CreateMap<Work, WorkCreateDto>().ReverseMap();


        }

    }
}
