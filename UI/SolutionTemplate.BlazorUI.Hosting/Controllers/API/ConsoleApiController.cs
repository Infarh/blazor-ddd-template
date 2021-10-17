using System;
using Microsoft.AspNetCore.Mvc;

namespace SolutionTemplate.BlazorUI.Hosting.Controllers.API
{
    [ApiController, Route("api/console")]
    public class ConsoleApiController : ControllerBase
    {
        [HttpGet("clear")]
        public void Clear() => Console.Clear();

        [HttpGet("line")]
        [HttpGet("line/{Message}")]
        [HttpGet("writeline")]
        [HttpGet("writeline/{Message}")]
        public void WriteLine(string Message) => Console.WriteLine(Message);
    }
}
