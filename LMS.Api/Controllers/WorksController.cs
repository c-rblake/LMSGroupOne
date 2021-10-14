using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Core.Repositories;
using LMS.Api.ResourceParamaters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if(await uow.CompleteAsync())
            {
                var workDto = mapper.Map<WorkDto>(workResult);
                //return CreatedAtAction(nameof(GetWork), workDto);
                return CreatedAtAction(nameof(GetWork), new { id = workDto.Id}, workDto);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


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
            return Ok(workDto);
        }

        [HttpGet(Name = "GetWorks")]
        //[HttpHead]
        public async Task<ActionResult<IEnumerable<WorkDto>>> GetWorks([FromQuery]WorksResourceParameters workResourceParameters)
        {
            var workResults = await uow.WorksRepository.GetAllWorksAsync(workResourceParameters);
            if (workResults is null) return NotFound();

            var workDtos = mapper.Map<IEnumerable<WorkDto>>(workResults);
            if(workDtos is null)
            {
                return StatusCode(500);
            }
            return Ok(workDtos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutWork(int id, WorkPutDto workPutDto)
        {
            var work = await uow.WorksRepository.FindAsync(id);

            if (workPutDto is null) return StatusCode(404);

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



    }
}
