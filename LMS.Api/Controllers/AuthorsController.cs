using LMS.Api.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController: ControllerBase
    {
        private readonly IUnitOfWork uow;

        public AuthorsController(IUnitOfWork uow)
        {
            this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }


    }
}
