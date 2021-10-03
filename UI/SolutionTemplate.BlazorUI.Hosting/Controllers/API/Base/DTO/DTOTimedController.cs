using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolutionTemplate.BlazorUI.Hosting.Models;
using SolutionTemplate.BlazorUI.Hosting.Models.DTO;
using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.BlazorUI.Hosting.Controllers.API.Base.DTO
{
    public class DTOTimedController<T, TTimedDTO> : DTOController<T, TTimedDTO>
        where T : ITimedEntity
        where TTimedDTO : DTOModel
    {
        protected ITimedRepository<T> TimedRepository => (ITimedRepository<T>)_Repository;

        public DTOTimedController(IRepository<T> Repository, IMapper Mapper, ILogger<DTOTimedController<T, TTimedDTO>> Logger) 
            : base(Repository, Mapper, Logger)
        {
        }

        [HttpGet("time/greater({ReferenceTime})/any")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> ExistGreaterThenTime(DateTimeOffset ReferenceTime)
        {
            var is_exists = await TimedRepository.ExistGreaterThenTime(ReferenceTime).ConfigureAwait(false);
            return Ok(is_exists);
        }

        [HttpGet("time/greater({ReferenceTime})")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TTimedDTO>>> GetAllGreaterThenTime(DateTimeOffset ReferenceTime)
        {
            var items = await TimedRepository.GetAllGreaterThenTime(ReferenceTime);
            var tdos = _Mapper.Map<IEnumerable<TTimedDTO>>(items);
            return Ok(tdos);
        }

        [HttpGet("time/greater({ReferenceTime})/items[[{Skip:int}:{Count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<TTimedDTO>> GetGreaterThenTime(DateTimeOffset ReferenceTime, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await TimedRepository.GetGreaterThenTime(ReferenceTime, Skip, Count, Cancel);
            var tdos = _Mapper.Map<IEnumerable<TTimedDTO>>(items);
            return tdos;
        }

        [HttpGet("time/greater({ReferenceTime})/page[[{PageIndex:int}/{PageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IPage<TTimedDTO>> GetPageGreaterThenTime(DateTimeOffset ReferenceTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var page = await TimedRepository.GetPageGreaterThenTime(ReferenceTime, PageIndex, PageSize, Cancel);
            var tdos = _Mapper.Map<IEnumerable<TTimedDTO>>(page.Items);
            var dtopage = new Page<TTimedDTO>(tdos, page.ItemsCount, page.TotalCount, page.PageNumber, page.PageSize);
            return dtopage;
        }

        [HttpGet("time/less({ReferenceTime})/any")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> ExistLessThenTime(DateTimeOffset ReferenceTime)
        {
            var is_exists = await TimedRepository.ExistLessThenTime(ReferenceTime).ConfigureAwait(false);
            return Ok(is_exists);
        }

        [HttpGet("time/less({ReferenceTime})")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TTimedDTO>>> GetAllLessThenTime(DateTimeOffset ReferenceTime)
        {
            var items = await TimedRepository.GetAllLessThenTime(ReferenceTime);
            var tdos = _Mapper.Map<IEnumerable<TTimedDTO>>(items);
            return Ok(tdos);
        }

        [HttpGet("time/less({ReferenceTime})/items[[{Skip:int}:{Count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<TTimedDTO>> GetLessThenTime(DateTimeOffset ReferenceTime, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await TimedRepository.GetLessThenTime(ReferenceTime, Skip, Count, Cancel);
            var tdos = _Mapper.Map<IEnumerable<TTimedDTO>>(items);
            return tdos;
        }

        [HttpGet("time/less({ReferenceTime})/page[[{PageIndex:int}/{PageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IPage<TTimedDTO>> GetPageLessThenTime(DateTimeOffset ReferenceTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var page = await TimedRepository.GetPageLessThenTime(ReferenceTime, PageIndex, PageSize, Cancel);
            var tdos = _Mapper.Map<IEnumerable<TTimedDTO>>(page.Items);
            var dtopage = new Page<TTimedDTO>(tdos, page.ItemsCount, page.TotalCount, page.PageNumber, page.PageSize);
            return dtopage;
        }
    }
}
