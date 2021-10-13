﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Course;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMSGroupOne.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CourseController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/course/create")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                uow.CourseRepository.AddCourse(mapper.Map<Course>(course));
                await uow.CompleteAsync();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}