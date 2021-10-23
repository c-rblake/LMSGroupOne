using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Course;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return PartialView();
        }

        [Route("/course/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = mapper.Map<CourseEditViewModel>(await uow.CourseRepository.FindAsync(id));
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("/course/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var course = await uow.CourseRepository.FindAsync(id);
                    mapper.Map(viewModel, course);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.CourseRepository.CourseExistById(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Home");
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CreateCourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                uow.CourseRepository.AddCourse(mapper.Map<Course>(course));
                await uow.CompleteAsync();
                course.Success = true;
                course.Message = "Course was created";
                course.ReturnId = 1234;  // todo return a valid id for the created course
            }
            else
            {
                course.Success = false;
                course.Message = "Course creation failed";
            }

            return PartialView(course);
        }

        public IActionResult Create()
        {
            
            return PartialView(new CreateCourseViewModel());
        }


        [HttpGet]
        public JsonResult VerifyCourseName(string Name)
        {            
            bool courseExists = uow.CourseRepository.CourseExist(Name);
            if (courseExists)
            {
                return Json($"A Course with name {Name} already exists.");
                //return Json(false);
            }
            return Json(true);
        }
    }
}
