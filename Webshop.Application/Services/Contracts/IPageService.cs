using Webshop.Application.Helpers;

namespace Webshop.Application.Services.Contracts;

public interface IPageService
{
    public (IEnumerable<T> list, PagingInfo pageInfo) GetPaging<T>(IEnumerable<T> list, int requestedPage);
}