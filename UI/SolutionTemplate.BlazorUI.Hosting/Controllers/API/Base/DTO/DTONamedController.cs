using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolutionTemplate.BlazorUI.Hosting.Models.DTO;
using SolutionTemplate.Interfaces.Base.Entities;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.BlazorUI.Hosting.Controllers.API.Base.DTO
{
    public class DTONamedController<T, TNamedDTO> : DTOController<T, TNamedDTO>
        where T : INamedEntity
        where TNamedDTO : DTOModel
    {
        public DTONamedController(IRepository<T> Repository, IMapper Mapper, ILogger<DTONamedController<T, TNamedDTO>> Logger)
            : base(Repository, Mapper, Logger)
        {
        }

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
        public async Task<ActionResult<TNamedDTO>> GetByName(string Name)
        {
            if (await ((INamedRepository<T>)_Repository).GetByName(Name) is not { } item)
                return NotFound();
            var dto = _Mapper.Map<TNamedDTO>(item);

            return Ok(dto);
        }

        [HttpDelete("name/{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TNamedDTO>> DeleteByName(string Name)
        {
            _Logger.LogInformation("Удаление сущности Name: {0}", Name);
            if (await ((INamedRepository<T>)_Repository).DeleteByName(Name) is not { } item)
            {
                _Logger.LogInformation("Удаление сущности Name: {0} - сущность не найдена", Name);
                return NotFound();
            }

            _Logger.LogInformation("Удаление сущности (Name: {0}) {1} - успешно", Name, item);

            var dto = _Mapper.Map<TNamedDTO>(item);

            return Ok(dto);
        }
    }
}
