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

        //[Route("/course/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            var model = mapper.Map<CourseEditViewModel>(await uow.CourseRepository.FindAsync(id));
            if (model == null)
            {
                model = new CourseEditViewModel
                {
                    Id = id,
                    Success=false,
                    Message="Could not edit Course!"
                };

                return PartialView(model);
            }

            model.Id = id;

            return PartialView(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        //[Route("/course/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseEditViewModel viewModel)
        {
            //if (id != viewModel.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    var course = await uow.CourseRepository.FindAsync(viewModel.Id);
                    mapper.Map(viewModel, course);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.CourseRepository.CourseExistById(viewModel.Id))
                    {

                        viewModel.Message = "Course not edited!";
                        viewModel.Success = false;
                        return PartialView(viewModel);
                        
                    }
                    else
                    {
                        throw;
                    }
                }

                viewModel.Success = true;
                viewModel.ReturnId = viewModel.Id;
                viewModel.Message = "Course was edited!";
                return PartialView(viewModel); // success
                
            }
            viewModel.Message = "Course not edited!";
            viewModel.Success = false;
            return PartialView(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Course course = mapper.Map<Course>(model);
                uow.CourseRepository.AddCourse(mapper.Map<Course>(course));
                uow.CompleteAsync().Wait();
                model.Success = true;
                model.Message = "Course was created";
                model.ReturnId = course.Id;  
            }
            else
            {
                model.Success = false;
                model.Message = "Course creation failed";
            }

            return PartialView(model);
        }

        public IActionResult Create()
        {
            var model = new CreateCourseViewModel
            {
                StartDate = DateTime.Now,
                EndDate=DateTime.Now
            };

            return PartialView(model);
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
