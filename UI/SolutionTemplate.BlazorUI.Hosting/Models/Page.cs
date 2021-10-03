using System.Collections.Generic;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.BlazorUI.Hosting.Models
{
    public record Page<T>(
        IEnumerable<T> Items, 
        int ItemsCount, 
        int TotalCount, 
        int PageNumber, 
        int PageSize) 
        : IPage<T>;
}
