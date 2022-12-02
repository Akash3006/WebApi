using Microsoft.EntityFrameworkCore;

namespace WebApi.Helpers
{
    public class PagedList<T>:List<T>
    {
        public PagedList(IEnumerable<T> items,int count,int pageSize,int pageNumber)
        {
            //Total Pages required
            TotalPages = (int) Math.Ceiling(count /(double)pageSize);
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = count;

            //Function to add list items
            AddRange(items);
        }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreatePageAsync(IQueryable<T> source , int pageNumber, int pageSize){
        //Take count of items
        var count = await source.CountAsync();

        //Incase page number 1 then notthing to show 
        //else pagenumber -1 * size
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        //return the list of new items when changing page number
        return new PagedList<T>(items,count,pageNumber,pageSize);
        }
    }
}