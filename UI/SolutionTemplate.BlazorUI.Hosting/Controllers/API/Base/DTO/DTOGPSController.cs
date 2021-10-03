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
    public class DTOGPSController<T, TGPSDTO> : DTOController<T, TGPSDTO>
        where T : IGPSEntity
        where TGPSDTO : DTOModel
    {
        protected IGPSRepository<T> GPSRepository => (IGPSRepository<T>)_Repository;

        public DTOGPSController(IRepository<T> Repository, IMapper Mapper, ILogger<DTOGPSController<T, TGPSDTO>> Logger)
            : base(Repository, Mapper, Logger)
        {
        }

        [HttpGet("exist/GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<ActionResult<bool>> ExistInLocation(double Latitude, double Longitude, double RangeInMeters)
        {
            var is_exist = await GPSRepository.ExistInLocation(Latitude, Longitude, RangeInMeters);
            return is_exist ? Ok(true) : NotFound(false);
        }

        [HttpGet("count/GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<int>> GetCountInLocation(double Latitude, double Longitude, double RangeInMeters)
        {
            var count = await GPSRepository.GetCountInLocation(Latitude, Longitude, RangeInMeters);
            return count;
        }

        [HttpGet("GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TGPSDTO>>> GetAllByLocationInRange(double Latitude, double Longitude, double RangeInMeters)
        {
            var items = await GPSRepository.GetAllByLocationInRange(Latitude, Longitude, RangeInMeters);
            var dtos = _Mapper.Map<IEnumerable<TGPSDTO>>(items);
            return Ok(dtos);
        }

        [HttpGet("GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})/items[[{Skip:int}:{Count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TGPSDTO>>> GetAllByLocationInRange(double Latitude, double Longitude, double RangeInMeters, int Skip, int Count)
        {
            var items = await GPSRepository.GetAllByLocationInRange(Latitude, Longitude, RangeInMeters, Skip, Count);
            var dtos = _Mapper.Map<IEnumerable<TGPSDTO>>(items);
            return Ok(items);
        }

        [HttpGet("GPS({Latitude:double}:{Longitude:double})/closest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TGPSDTO>> GetByLocation(double Latitude, double Longitude)
        {
            var item = await GPSRepository.GetByLocation(Latitude, Longitude);
            var dto = _Mapper.Map<TGPSDTO>(item);
            return dto;
        }

        [HttpGet("GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})/closest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TGPSDTO>> GetByLocationInRange(double Latitude, double Longitude, double RangeInMeters)
        {
            if (await GPSRepository.GetByLocationInRange(Latitude, Longitude, RangeInMeters) is not { } item)
                return NotFound();
            var dto = _Mapper.Map<TGPSDTO>(item);
            return Ok(dto);
        }

        [HttpGet("GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})/page[[{PageNumber:int}/{PageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IPage<TGPSDTO>>> GetPageByLocationInRange(double Latitude, double Longitude, double RangeInMeters, int PageNumber, int PageSize)
        {
            var page = await GPSRepository.GetPageByLocationInRange(Latitude, Longitude, RangeInMeters, PageNumber, PageSize);
            var dtos = _Mapper.Map<IEnumerable<TGPSDTO>>(page.Items);
            var dto_page = new Page<TGPSDTO>(dtos, page.ItemsCount, page.TotalCount, page.PageNumber, page.PageSize);
            return Ok(dto_page);
        }

        [HttpDelete("GPS({Latitude:double}:{Longitude:double})/closest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TGPSDTO>> DeleteByLocation(double Latitude, double Longitude)
        {
            _Logger.LogInformation("Удаление записи по координатам lat:{0} - lon:{1}", Latitude, Longitude);
            if (await GPSRepository.DeleteByLocation(Latitude, Longitude) is not { } item)
            {
                _Logger.LogInformation("Удаление записи по координатам lat:{0} - lon:{1} - не выполнено: запись не найдена", Latitude, Longitude);
                return NotFound();
            }
            _Logger.LogInformation("Удаление записи по координатам lat:{0} - lon:{1} - выполнено успешно", Latitude, Longitude);

            var dto = _Mapper.Map<TGPSDTO>(item);
            return Ok(dto);
        }

        [HttpDelete("GPS({Latitude:double}:{Longitude:double})/range({RangeInMeters:double})")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TGPSDTO>> DeleteByLocationInRange(double Latitude, double Longitude, double RangeInMeters)
        {
            _Logger.LogInformation("Удаление записи по координатам lat:{0} - lon:{1} - range:{2}", Latitude, Longitude, RangeInMeters);
            if (await GPSRepository.DeleteByLocationInRange(Latitude, Longitude, RangeInMeters) is not { } item)
            {
                _Logger.LogInformation("Удаление записи по координатам lat:{0} - lon:{1} - range:{2} - не выполнено: запись не найдена", Latitude, Longitude, RangeInMeters);
                return NotFound();
            }
            _Logger.LogInformation("Удаление записи по координатам lat:{0} - lon:{1} - range:{2} - выполнено успешно", Latitude, Longitude, RangeInMeters);

            var dto = _Mapper.Map<TGPSDTO>(item);
            return Ok(dto);
        }
    }
}
