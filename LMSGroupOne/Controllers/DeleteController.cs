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





        public IActionResult DeleteCourse(int id)
        {            
            
            var model = new DeleteModelView
            {
                Id = id,
                Name = "courseName",
                Message = "",
                ReturnId = 0,
                Success = false

            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(DeleteModelView inp)
        {
            
            int id = inp.Id;
            await uow.CourseRepository.RemoveAsync(id);
            await uow.CompleteAsync();
            inp.Message = "Course Deleted!";
            inp.ReturnId = id;
            inp.Success = true;

            return PartialView(inp);
        }





        public IActionResult DeleteModule(int id)
        {
            Debug.WriteLine("id from controller entry delete module "+id);

            var model = new DeleteModelView
            {
                Id = id,
                Name = "-",
                Message = "Hello world",
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

            Debug.WriteLine("id from controller final delete module " + id);

            await uow.ModuleRepository.RemoveAsync(id);
            await uow.CompleteAsync();
            inp.Message = "Module Deleted!";
            inp.ReturnId = id;
            inp.Success = true;

            return PartialView(inp);
        }





        public IActionResult DeleteActivity(int id)
        {
            var model = new DeleteModelView
            {
                Id=id,
                Name = "courseName",
                Message = "Hello world",
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
