using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.BlazorUI.Hosting.Controllers.API.Base
{
    public abstract class TimedEntityController<T> : EntityController<T> where T : ITimedEntity, IGPSEntity<int>
    {
        protected ITimedRepository<T> TimedRepository => (ITimedRepository<T>) _Repository;

        protected TimedEntityController(ITimedRepository<T> Repository, ILogger<EntityController<T>> Logger) : base(Repository, Logger) { }

        [HttpGet("time/greater({ReferenceTime})/any")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> ExistGreaterThenTime(DateTimeOffset ReferenceTime)
        {
            var is_exists = await TimedRepository.ExistGreaterThenTime(ReferenceTime).ConfigureAwait(false);
            return Ok(is_exists);
        }

        [HttpGet("time/greater({ReferenceTime})")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> GetAllGreaterThenTime(DateTimeOffset ReferenceTime)
        {
            var items = await TimedRepository.GetAllGreaterThenTime(ReferenceTime);
            return Ok(items);
        }

        [HttpGet("time/greater({ReferenceTime})/items[[{Skip:int}:{Count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<T>> GetGreaterThenTime(DateTimeOffset ReferenceTime, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await TimedRepository.GetGreaterThenTime(ReferenceTime, Skip, Count, Cancel);
            return items;
        }

        [HttpGet("time/greater({ReferenceTime})/page[[{PageIndex:int}/{PageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IPage<T>> GetPageGreaterThenTime(DateTimeOffset ReferenceTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var page = await TimedRepository.GetPageGreaterThenTime(ReferenceTime, PageIndex, PageSize, Cancel);
            return page;
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
        public async Task<ActionResult<IEnumerable<T>>> GetAllLessThenTime(DateTimeOffset ReferenceTime)
        {
            var items = await TimedRepository.GetAllLessThenTime(ReferenceTime);
            return Ok(items);
        }

        [HttpGet("time/less({ReferenceTime})/items[[{Skip:int}:{Count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<T>> GetLessThenTime(DateTimeOffset ReferenceTime, int Skip, int Count, CancellationToken Cancel = default)
        {
            var items = await TimedRepository.GetLessThenTime(ReferenceTime, Skip, Count, Cancel);
            return items;
        }

        [HttpGet("time/less({ReferenceTime})/page[[{PageIndex:int}/{PageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IPage<T>> GetPageLessThenTime(DateTimeOffset ReferenceTime, int PageIndex, int PageSize, CancellationToken Cancel = default)
        {
            var page = await TimedRepository.GetPageLessThenTime(ReferenceTime, PageIndex, PageSize, Cancel);
            return page;
        }
    }
}
