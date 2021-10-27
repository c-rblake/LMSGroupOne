using LMSGroupOne.Models.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            var model = new SearchModelView
            {

            };


            return PartialView("Search", model);
        }


        public IActionResult Search(SearchModelView input)
        {

            var model = new SearchModelView
            {
                Name = input.Name,
                Courses=new string[] 
                { 
                    "Kajsa anka",
                    "Joakim von Anka",
                    "Paul Anka"
                }
            };


            return PartialView(model);
        }
    }
}
