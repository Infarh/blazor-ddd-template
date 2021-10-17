using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SolutionTemplate.BlazorUI.Hosting.Controllers.API
{
    [ApiController, Route("api/log")]
    public class LogApiController : ControllerBase
    {
        private readonly ILogger<LogApiController> _Logger;

        public LogApiController(ILogger<LogApiController> Logger) => _Logger = Logger;

        [HttpGet("trace"), HttpGet("trace/{Message}")]
        public void LogTrace(string Message) => _Logger.LogTrace(Message);

        [HttpGet("debug"), HttpGet("debug/{Message}")]
        public void LogDebug(string Message) => _Logger.LogDebug(Message);

        [HttpGet("inforamtion"), HttpGet("inforamtion/{Message}")]
        public void LogInformation(string Message) => _Logger.LogInformation(Message);
        
        [HttpGet("warning"), HttpGet("warning/{Message}")]
        public void LogWarning(string Message) => _Logger.LogWarning(Message);
        
        [HttpGet("error"), HttpGet("error/{Message}")]
        public void LogError(string Message) => _Logger.LogError(Message);
        
        [HttpGet("critical"), HttpGet("critical/{Message}")]
        public void LogCritical(string Message) => _Logger.LogCritical(Message);
    }
}
