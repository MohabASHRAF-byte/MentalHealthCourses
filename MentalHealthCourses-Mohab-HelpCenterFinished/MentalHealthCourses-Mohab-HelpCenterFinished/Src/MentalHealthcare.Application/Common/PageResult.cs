using System.Globalization;

namespace MentalHealthcare.Application.Common;

public class PageResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPages { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    public string SortBy { get; set; } //Added By Marsoo

    

    public PageResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = ((totalCount + pageSize - 1) / pageSize);
        ItemsFrom = Math.Min(totalCount, pageSize * (pageNumber - 1) + 1);
        ItemsTo = Math.Min(totalCount, ItemsFrom + pageSize - 1);
    }

    //written by Marslino
    public PageResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber, string? sortBy)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = ((totalCount + pageSize - 1) / pageSize);
        ItemsFrom = Math.Min(totalCount, pageSize * (pageNumber - 1) + 1);
        ItemsTo = Math.Min(totalCount, ItemsFrom + pageSize - 1);
        SortBy = sortBy;
    }










}