using Webshop.Application.Helpers;

namespace Webshop.Application.Services.Contracts;

public interface IPageService
{
    public Tuple<IEnumerable<T>, PagingInfo> GetPaging<T>(IEnumerable<T> list, int requestedPage);
}