using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Models.ViewModels.Course;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Core.Models.Dto;
using LMS.Core.Models.ViewModels.API.Work;
using LMS.Core.Models.Entities.API;
using LMS.Core.Models.ViewModels.Module;
using LMS.Core.Models.ViewModels;
using LMS.Core.Models.ViewModels.API.Author;

namespace LMS.Data.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, CreateCourseViewModel>().ReverseMap();
            CreateMap<Course, CourseEditViewModel>().ReverseMap();

            CreateMap<Module, CreateModuleViewModel>().ReverseMap();
            CreateMap<Module, EditModuleViewModel>().ReverseMap();

            CreateMap<Activity, ActivityCreateViewModel>().ReverseMap();
            CreateMap<Activity, ActivityEditViewModel>().ReverseMap();
            CreateMap<Person, AccountCreateViewModel>().ReverseMap();
            CreateMap<AuthorsViewModel, AuthorDto >().ReverseMap();
            CreateMap<WorksViewModel, WorkDto>().ReverseMap();
            CreateMap<Work, WorkDto>().ReverseMap();
            CreateMap<WorkAuthorDto, Author>().ReverseMap();


            CreateMap<AuthorWorksViewModel, WorkAuthorDto>().ReverseMap();

            CreateMap<Person, AccountCreateViewModel>().ReverseMap();
            CreateMap<Person, AccountEditViewModel>().ReverseMap();
        }
    }
}