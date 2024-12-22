namespace MentalHealthcare.Application.Common;

public class PageResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPages { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    public string SortedBy { get; set; } //Added By Marslinooo


    public PageResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = ((totalCount + pageSize - 1) / pageSize);
        ItemsFrom = Math.Min(totalCount, pageSize * (pageNumber - 1) + 1);
        ItemsTo = Math.Min(totalCount, ItemsFrom + pageSize - 1);

    }

    public PageResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber, string? sortedBy = null)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (totalCount + pageSize - 1) / pageSize;
        ItemsFrom = Math.Min(totalCount, pageSize * (pageNumber - 1) + 1);
        ItemsTo = Math.Min(totalCount, ItemsFrom + pageSize - 1);
        SortedBy = sortedBy ?? "Unsorted";
    }
}