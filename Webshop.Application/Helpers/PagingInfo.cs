namespace Webshop.Application.Helpers;

public class PagingInfo
{
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public Filters Filters { get; set; }
}

public class Filters
{
    public int? Category { get; set; }
}