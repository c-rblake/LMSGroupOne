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
    public class TeacherController:Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public TeacherController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

       /* public async Task<IActionResult> Index()
        {
            return null;
        }*/

    }
}
