using AutoMapper;
using LMS.Api.Core.Dtos;
using LMS.Api.Core.Entities;
using LMS.Api.Core.Repositories;
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


        [HttpGet("{id}", Name =("GetWork"))] //ToDo change to NAME Case Senestive for Swagger.
        public async Task<ActionResult<WorkDto>> GetWork(int id)
        {
            var workResult = await uow.WorksRepository.GetWorkAsync(id);
            if (workResult is null) return NotFound();
            //TODO mapping etc.

            var workDto = mapper.Map<WorkDto>(workResult);
            if(workDto is null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(workDto);
        }


}
}
