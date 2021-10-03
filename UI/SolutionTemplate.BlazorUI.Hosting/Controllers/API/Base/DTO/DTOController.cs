using System;
using System.Collections.Generic;
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
    [ApiController, Route("api/[controller]")]
    public abstract class DTOController<T, TDTO> : ControllerBase 
        where T : IEntity 
        where TDTO : DTOModel
    {
        protected readonly IRepository<T> _Repository;
        protected readonly IMapper _Mapper;
        protected readonly ILogger<DTOController<T, TDTO>> _Logger;

        protected DTOController(IRepository<T> Repository, IMapper Mapper, ILogger<DTOController<T, TDTO>> Logger)
        {
            _Repository = Repository;
            _Mapper = Mapper;
            _Logger = Logger;
        }

        /// <summary>Проверка на пустоту</summary>
        /// <returns>Истина, если <typeparamref name="T"/> отсутствуют</returns>
        [HttpGet("isempty"), ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> IsEmpty()
        {
            var is_empty = await _Repository.IsEmpty();
            return Ok(is_empty);
        }

        /// <summary>Число <typeparamref name="T"/> в репозитории</summary>
        /// <returns>Число <typeparamref name="T"/> в репозитории</returns>
        [HttpGet("count"), ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<int>> GetItemsCount()
        {
            var count = await _Repository.GetCount();
            return Ok(count);
        }

        /// <summary>Проверка наличия <typeparamref name="T"/> с заданным <paramref name="id"/></summary>
        /// <param name="id">Идентификатор <typeparamref name="T"/> <paramref name="id"/> существует</param>
        /// <returns>Истина, если <typeparamref name="T"/> с указанным </returns>
        [HttpGet("exist/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistId(int id)
        {
            var exist_id = await _Repository.ExistId(id);
            return exist_id ? Ok(true) : NotFound(false);
        }

        [HttpGet("exist")]
        [HttpPost("exist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(T item)
        {
            var exist = await _Repository.Exist(item);
            return exist ? Ok(true) : NotFound(false);
        }

        [HttpGet, ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TDTO>>> GetAll()
        {
            var items = await _Repository.GetAll();
            var dtos = _Mapper.Map<IEnumerable<TDTO>>(items);
            return Ok(dtos);
        }

        [HttpGet("items[[{Skip:int}:{Count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TDTO>>> Get(int Skip, int Count)
        {
            var items = await _Repository.Get(Skip, Count);
            var dtos = _Mapper.Map<IEnumerable<TDTO>>(items);
            return Ok(dtos);
        }

        [HttpGet("page/{PageIndex:int}/{PageSize:int}")]
        [HttpGet("page[[{PageIndex:int}/{PageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(int))]
        public async Task<ActionResult<IPage<TDTO>>> GetPage(int PageIndex, int PageSize)
        {
            var result = await _Repository.GetPage(PageIndex, PageSize);
            var dtos = _Mapper.Map<IEnumerable<TDTO>>(result.Items);
            var page = new Page<TDTO>(dtos, result.ItemsCount, result.TotalCount, result.PageNumber, result.PageSize);
            return result.ItemsCount > 0 ? Ok(page) : NotFound(page);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Get(int id)
        {
            var item = await _Repository.GetById(id);
            var dtos = _Mapper.Map<IEnumerable<TDTO>>(item);
            return item is not null ? Ok(dtos) : NotFound();
        }

        [HttpPost, ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(T dto)
        {
            var item = _Mapper.Map<T>(dto);

            _Logger.LogInformation("Добавление {0}...", item);
            var result = await _Repository.Add(item);

            _Logger.LogInformation("Добавление {0} выполнено успешно id: {1}", item, item.Id);
            return CreatedAtAction("", new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(T dto)
        {
            var item = _Mapper.Map<T>(dto);

            _Logger.LogInformation("Обновление (id: {0}){1}...", item.Id, item);
            if (await _Repository.Update(item) is not { } result)
            {
                _Logger.LogInformation("Сущность (id: {0}){1} не найдена", item.Id, item);
                return NotFound(item);
            }

            _Logger.LogInformation("Обновление (id: {0}){1} выполнено успешно", item.Id, item);
            return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(T dto)
        {
            var item = _Mapper.Map<T>(dto);

            try
            {
                _Logger.LogInformation("Удаление (id: {0}){1}...", item.Id, item);
                if (await _Repository.Delete(item) is not { } result)
                {
                    _Logger.LogInformation("Сущность (id: {0}){1} не найдена", item.Id, item);
                    return NotFound();
                }

                _Logger.LogInformation("Удаление (id: {0}){1} выполнено успешно", item.Id, item);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            _Logger.LogInformation("Удаление сущности id: {0}...", id);
            if (await _Repository.DeleteById(id) is not { } result)
            {
                _Logger.LogInformation("Сущность id: {0} не найдена", id);
                return NotFound();
            }

            _Logger.LogInformation("Удаление (id: {0}){1} выполнено успешно", result.Id, result);
            return Ok(result);
        }
    }
}
