using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.BlazorUI.Hosting.Controllers.API.Base
{
    /// <summary>Контроллер именованныйх сущностей</summary>
    /// <typeparam name="T">Тип именованной сущности</typeparam>
    public abstract class NamedEntityController<T> : EntityController<T> where T : INamedEntity
    {
        protected NamedEntityController(INamedRepository<T> Repository, ILogger<EntityController<T>> Logger) : base(Repository, Logger) { }

        [HttpGet("exist/name/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistName(string Name)
        {
            var is_exists = await ((INamedRepository<T>)_Repository).ExistName(Name);
            return is_exists ? Ok(true) : NotFound(false);
        }

        [HttpGet("name/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> GetByName(string Name)
        {
            if (await ((INamedRepository<T>)_Repository).GetByName(Name) is not { } item)
                return NotFound();
            return Ok(item);
        }

        [HttpDelete("name/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> DeleteByName(string Name)
        {
            _Logger.LogInformation("Удаление сущности Name: {0}", Name);
            var item = await ((INamedRepository<T>)_Repository).DeleteByName(Name);
            if (item is not { } result)
            {
                _Logger.LogInformation("Удаление сущности Name: {0} - сущность не найдена", Name);
                return NotFound();
            }

            _Logger.LogInformation("Удаление сущности (Name: {0}) {1} - успешно", Name, result);
            return result;
        }
    }
}
