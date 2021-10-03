using System;

namespace SolutionTemplate.BlazorUI.Hosting.Models.DTO
{
    public abstract class DTOTimedEntity : DTOModel
    {
        public DateTimeOffset Time { get; set; }
    }
}
