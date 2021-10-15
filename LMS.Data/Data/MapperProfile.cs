using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Models.ViewModels.Course;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Data.Data
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, CreateCourseViewModel>().ReverseMap();

            CreateMap<Activity, ActivityCreateViewModel>().ReverseMap();
        }
    }
}
