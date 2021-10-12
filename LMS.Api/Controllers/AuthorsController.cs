using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Repositories;
using LMS.Api.ResourceParamaters;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper mapper;

        public AuthorsController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
            this.mapper = mapper;
        }

        [HttpGet("{Id}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var result = await uow.AuthorRepository.GetAuthorAsync(id); //ToDo the Query has TWO Awaits total..
            if (result is null) return NotFound();
            var dtoResult = mapper.Map<AuthorDto>(result);
            if (result is null) return StatusCode(500);

            return Ok(dtoResult);
        }


        //GET | api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(AuthorsResourceParameters authorResourceParameters)
        {
            
            return null;
        }
        //ToDo One Million API CRUDS Dont forget to USE uow



        [HttpPatch("{id}")]
        public async Task<ActionResult<AuthorDto>> PatchAuthor(int id, JsonPatchDocument<AuthorDto> patchDocument) //ToDo PatchAuthorDto
        {
            throw new NotImplementedException(nameof(PatchAuthor));
        }
    }
}
