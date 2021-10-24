using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMSGroupOne.Models.Delete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class DeleteController : Controller
    {

        private readonly IJTUnitOfWork uow;
        private readonly UserManager<Person> userManager;
        public DeleteController(IJTUnitOfWork uow, UserManager<Person> userManager)
        {
            this.uow = uow;

            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            //userManager.GetUsersInRoleAsync("RoleName");

        }





        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course=await uow.CourseRepository.GetCourse(id);

            var model = new DeleteModelView
            {
                Id = id,
                Name = course.Name,
                Message = "Delete this Course?",
                ReturnId = 0,
                Success = false

            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(DeleteModelView inp)
        {
            // todo check for success
            int id = inp.Id;
            await uow.CourseRepository.RemoveAsync(id);
            await uow.CompleteAsync();
            
            inp.Message = "Course Deleted!";
            inp.ReturnId = id;
            inp.Success = true;

            return PartialView(inp);
        }





        public async Task<IActionResult> DeleteModule(int id)
        {
            var modul = await uow.ModuleRepository.GetModule(id);

            var model = new DeleteModelView
            {
                Id = id,
                Name = modul.Name,
                Message = "Delete this Module?",
                ReturnId = 0,
                Success = false

            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteModule(DeleteModelView inp)
        {
            int id = inp.Id;
                        
            await uow.ModuleRepository.RemoveAsync(id);
            await uow.CompleteAsync();
            inp.Message = "Module Deleted!";
            inp.ReturnId = id;
            inp.Success = true;

            return PartialView(inp);
        }





        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await uow.ActivityRepository.GetActivity(id);

            var model = new DeleteModelView
            {
                Id=id,
                Name = activity.Name,
                Message = "Delete this Activity?",
                ReturnId = 0,
                Success = false

            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(DeleteModelView inp)
        {
            int id = inp.Id;
            await uow.ActivityRepository.RemoveAsync(id);
            await uow.CompleteAsync();
            inp.Message = "Activity Deleted!";
            inp.ReturnId = id;
            inp.Success = true;

            return PartialView(inp);
        }

    }
}
