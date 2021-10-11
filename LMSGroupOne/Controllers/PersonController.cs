using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels;
using LMS.Core.Models.ViewModels.Course;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMSGroupOne.Controllers
{
    public class PersonController:Controller
    {
        private readonly IUnitOfWorks uow;
        private readonly IMapper mapper;

        public PersonController(IUnitOfWorks uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

       /* public async Task<IActionResult> Index()
        {
            return null;
        }*/

        [HttpPost]
        [Route("/teacher/course")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseViewModel course)
        {
            if (ModelState.IsValid)
            {
               uow.TeacherRepository.AddCourse(mapper.Map<Course>(course));
               await  uow.CompleteAsync();
           }

            return RedirectToAction(nameof(Index),"Home");
        }
    }
}
