using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Models.ViewModels.Course;

namespace LMS.Data.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, CreateCourseViewModel>().ReverseMap();
            CreateMap<Person, CreateAccountViewModel>()
                //.ForMember(destination => destination.Role,
                //opt => opt.MapFrom(source => source.Role.ToString()))
                //.ForMember(destination => destination.Password,
                //opt => opt.MapFrom(source => source.Password.ToString()))
            .ReverseMap();
        }
    }
}
