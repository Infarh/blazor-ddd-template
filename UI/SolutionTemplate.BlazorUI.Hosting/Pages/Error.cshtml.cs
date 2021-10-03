using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SolutionTemplate.BlazorUI.Hosting.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _Logger;
        public string RequestId { get; set; }
        public bool ShowRequestId => RequestId is { Length: >0 };

        public ErrorModel(ILogger<ErrorModel> Logger) => _Logger = Logger;

        public void OnGet() => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}
