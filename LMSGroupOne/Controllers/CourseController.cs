using System;
using System.Collections.Generic;
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
            return View();
        }

        [Route("/course/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await uow.CourseRepository.FindAsync(id);
            ViewBag.courseName = course.Name;
            var model = mapper.Map<CourseEditViewModel>(course);
            
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
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
            }

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult VerifyCourseName(string Name)
        {
            bool courseExists = uow.CourseRepository.CourseExist(Name);
            if (courseExists)
            {
                return Json($"A Course with name {Name} already exists.");
            }
            return Json(true);
        }
    }
}
