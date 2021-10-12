using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Core.Repositories;
using LMS.Api.ResourceParamaters;
using Microsoft.AspNetCore.Http;
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
        
        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id, bool includeWorks)
        {
            var result = await uow.AuthorRepository.GetAuthorAsync(id, includeWorks); //ToDo the Query has TWO Awaits total..
            if (result is null) return NotFound();
            var dtoResult = mapper.Map<AuthorDto>(result); //Result has the List of Works but Circlar reference.
            dtoResult.WorkDtos = mapper.Map<ICollection<AuthorWorkDto>>(result.Works);
            if (result is null) return StatusCode(500);

            return Ok(dtoResult);
        }


        //GET | api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery]AuthorsResourceParameters authorResourceParameters)
        {
            List<AuthorDto> dtoAuthors = new List<AuthorDto>();
            //Todo Implement. Should return AuthorDto with collection WorksDto
            var result = await uow.AuthorRepository.GetAllAuthorsAsync(authorResourceParameters);
            //For each result Author + Collection WorksDto
            if (result is null) return NotFound();
            if(authorResourceParameters.includeWorks)
            {
                foreach (var author in result)
                {
                    var dtoAuthor = mapper.Map<AuthorDto>(author);
                    dtoAuthor.WorkDtos = mapper.Map<ICollection<AuthorWorkDto>>(author.Works);
                    dtoAuthors.Add(dtoAuthor);
                };
            }
            else
            {
                dtoAuthors = mapper.Map<List<AuthorDto>>(result);
            }
            if (dtoAuthors is null) return StatusCode(500);

            return Ok(dtoAuthors);
            
        }
        //ToDo One Million API CRUDS Dont forget to USE uow

        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(AuthorCreateDto dto)
        {
            //TODO implment GetAuthorNameAsync Check
            //if (await uow.AuthorRepository.GetAuthorNameAsync(dto.FirstName, dto.LastName) != null)
            //{
            //    ModelState.AddModelError("FirstName", "Author with this First Name and Last name is in use");
            //    ModelState.AddModelError("LastName", "Author with this First Name and Last name is in use");
            //    return BadRequest(ModelState);
            //    //Todo, redirect to that author?
            //}
            var author = mapper.Map<Author>(dto);
            await uow.AuthorRepository.AddAsync(author);

            if(await uow.CompleteAsync())
            {
                var authorDto = mapper.Map<AuthorDto>(author);
                return CreatedAtAction(nameof(GetAuthor), new { id = authorDto.Id }, author);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpPatch("{id}")]
        public async Task<ActionResult<AuthorDto>> PatchAuthor(int id, JsonPatchDocument<AuthorCreateDto> patchDocument) //ToDo PatchAuthorDto
        {
            var author = await uow.AuthorRepository.GetAuthorAsync(id, false);
            if (author is null) return NotFound();

            var dto = mapper.Map<AuthorCreateDto>(author);
            
            patchDocument.ApplyTo(dto, ModelState); //ModelState.Values

            if (!TryValidateModel(dto)) return BadRequest();

            mapper.Map(dto, author); // from dto to author

            if (await uow.CompleteAsync())
            {
                return Ok(mapper.Map<AuthorDto>(author));
            }
            else
            {
                return StatusCode(500);
            }
                 
        }
    }
}
