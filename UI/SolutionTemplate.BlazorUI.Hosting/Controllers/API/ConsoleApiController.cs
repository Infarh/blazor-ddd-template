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
        public void WriteLine(string Message) => Console.WriteLine(Message);
    }
}
