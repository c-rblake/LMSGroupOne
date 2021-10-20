using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Core.Repositories;
using LMS.Api.Helpers;
using LMS.Api.ResourceParamaters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMS.Api.Controllers
{
    [Route("api/works")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;


        public WorksController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Work>> CreateWork(WorkCreateDto dto)
        {
            var workResult = mapper.Map<Work>(dto);
            await uow.WorksRepository.AddAsync(workResult);

            if (await uow.CompleteAsync())
            {
                var workDto = mapper.Map<WorkDto>(workResult);
                //return CreatedAtAction(nameof(GetWork), workDto);
                return CreatedAtAction(nameof(GetWork), new { id = workDto.Id }, workDto);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        //Post a Work to an Author Not in use.
        //[Route("api/works/{authorId}/works")]
        //[HttpPost(Name = "CreateWorkAtAuthor")]
        //public async Task<ActionResult<Work>> CreateWorkAtAuthor(int authorId, WorkCreateDto dto)
        //{
        //    var author = await uow.AuthorRepository.FindAsync(authorId);
        //    if (author is null) return BadRequest();
        //    if (dto is null) return BadRequest();

        //    var work = mapper.Map<Work>(dto);
        //    author.Works.Add(work);

        //    if(await uow.CompleteAsync())
        //    {
        //        var workDto = mapper.Map<WorkDto>(work);
        //        return CreatedAtAction(nameof(GetWork), new { id = workDto.Id }, workDto);
        //    }
        //    else
        //    {
        //        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    }
        //}


        [HttpGet("{id}", Name ="GetWork")] //ToDo change to NAME Case Senestive for Swagger.
        public async Task<ActionResult<WorkDto>> GetWork(int id)
        {
            var workResult = await uow.WorksRepository.GetWorkAsync(id);
            if (workResult is null) return NotFound();

            var workDto = mapper.Map<WorkDto>(workResult);
            if(workDto is null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            var customHeader = new { text = "text" };
            Response.Headers.Add("My Custom Header from GetWork", JsonSerializer.Serialize(customHeader));

            return Ok(workDto);
        }

        [HttpOptions]
        public IActionResult GetWorkOptions()
        {
            Response.Headers.Add("Allow","GET,OPTIONS,POST,PATCH");
            return Ok();
        }


        [HttpGet(Name = "GetWorks")]
        //[HttpHead]
        public async Task<ActionResult<IEnumerable<WorkDto>>> GetWorks([FromQuery]WorksResourceParameters workResourceParameters)
            //Async is in the Paged List Object.
        {
            //var workResults = await uow.WorksRepository.GetAllWorksAsync(workResourceParameters);
            var workResults = await uow.WorksRepository.GetAllWorksAsync(workResourceParameters);
            if (workResults is null) return NotFound();

            var workDtos = mapper.Map<IEnumerable<WorkDto>>(workResults);
            if(workDtos is null)
            {
                return StatusCode(500);
            }
            var previousPageLink = workResults.HasPrevious ?
                CreateWorkResourceUri(workResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = workResults.HasNext ?
                CreateWorkResourceUri(workResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = workResults.TotalCount,
                pagesize = workResults.PageSize,
                currentPage = workResults.CurrentPage,
                totalPages = workResults.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", 
                JsonSerializer.Serialize(paginationMetadata));


            return Ok(workDtos);
        }
        /// <summary>
        /// Will not be implemented there are many Foreign Keys and sublists.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workPutDto"></param>
        /// <returns>Db/backend Exception</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutWork(int id, WorkPutDto workPutDto)
        {
            if (workPutDto is null) return StatusCode(404);

            var work = await uow.WorksRepository.FindAsync(id);

            mapper.Map(workPutDto, work); //Map ONTO work. waiting in Context.

            if (await uow.CompleteAsync())
            {
                return Ok(mapper.Map<WorkDto>(work));
            }
            else
            {
                return StatusCode(500);
            }
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchWork(int id, JsonPatchDocument<WorkPatchDto> patchDocument)
        {
            if (patchDocument is null) return StatusCode(404);
            var work = await uow.WorksRepository.FindAsync(id);

            var dto = mapper.Map<WorkPatchDto>(work);

            patchDocument.ApplyTo(dto, ModelState);

            if (!TryValidateModel(dto)) return BadRequest(ModelState);

            mapper.Map(dto, work);

            if (await uow.CompleteAsync()) return Ok(mapper.Map<WorkDto>(work));
            else return StatusCode(500);


        }
        private string CreateWorkResourceUri(WorksResourceParameters worksResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetWorks",
                        new
                        {
                            pageNumber = worksResourceParameters.PageNumber - 1,
                            pageSize = worksResourceParameters.PageSize,
                            orderBy = worksResourceParameters.OrderBy,
                            //Todo add more
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetWorks",
                        new 
                        {
                            pageNumber = worksResourceParameters.PageNumber + 1,
                            pageSize = worksResourceParameters.PageSize,
                            orderBy = worksResourceParameters.OrderBy,
                        });

                //case ResourceUriType.Current:
                default:
                    return Url.Link("GetWorks",
                        new
                        {
                            pageNumber = worksResourceParameters.PageNumber - 1,
                            pageSize = worksResourceParameters.PageSize,
                            orderBy = worksResourceParameters.OrderBy,
                        });

            }
        }



    }
}
