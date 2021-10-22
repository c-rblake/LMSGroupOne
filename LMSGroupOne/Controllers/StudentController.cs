using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class StudentController:Controller
    {

        [Authorize]
        public IActionResult Index()
        {
            //return RedirectToAction("Index", "MainNavigation");
            return RedirectToAction("StudentHome", "MainNavigation");
            //return View("MainNavigation/StudentHome"); 
            //return View("~/MainNavigation/StudentHome.cshtml");
            //return View("~/MainNavigation/StudentHome.cshtml");
            //return RedirectToAction("Index", "MainNavigation");

        }

    }
}
