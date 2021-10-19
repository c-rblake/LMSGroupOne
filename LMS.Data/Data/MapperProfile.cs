using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Models.ViewModels;
using LMS.Core.Models.ViewModels.Module;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Models.ViewModels.Course;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            CreateMap<Person, CreateAccountViewModel>().ReverseMap();
        }
    }
}
