using Webshop.Application.Helpers;
using Webshop.Application.Services.Contracts;

namespace Webshop.Application.Services;

public class PageService : IPageService
{
    private const int PageSize = 18;

    /// <summary>
    /// returns a tuple with the two objects for pagination
    /// </summary>
    /// <param name="list">A list of Entities</param>
    /// <param name="requestedPage">the number of the requested page (start with 1)</param>
    /// <typeparam name="T">The Entity for which the page is made</typeparam>
    /// <returns>a tuple with first the list of entities for the current page and
    /// second an object holding the info (metadata) for the current page</returns>
    public (IEnumerable<T> list, PagingInfo pageInfo) GetPaging<T>(IEnumerable<T> list, int requestedPage)
    {
        var enumerable = list.ToList();
        var totalCount = enumerable.Count;

        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        requestedPage = Math.Clamp(requestedPage, 1, Math.Max(1, totalPages));

        list = enumerable.Skip((requestedPage - 1) * PageSize).Take(PageSize);

        var pageInfo = new PagingInfo
        {
            CurrentPage = requestedPage,
            TotalPages = totalPages
        };
        return (list, pageInfo);
    }
}