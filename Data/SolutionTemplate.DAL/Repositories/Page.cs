using System.Collections.Generic;
using SolutionTemplate.Interfaces.Base.Repositories;

namespace SolutionTemplate.DAL.Repositories
{
    /// <summary>Страница</summary>
    /// <typeparam name="T">Тип элементов страницы</typeparam>
    internal record Page<T>(IEnumerable<T> Items, int ItemsCount, int TotalCount, int PageNumber, int PageSize) : IPage<T>;
}
