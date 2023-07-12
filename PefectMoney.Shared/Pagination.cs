using System.Linq;

namespace Shared;

public static class Pagination
{
    //public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, long page, long pageSize,
    //    out long recordCount)
    //{
    //    recordCount = source.Count();
    //    return source.Skip((int)((page - 1) * pageSize)).Take((int)pageSize);
    //}

    public static IQueryable<TSource> ToPaged<TSource>(this IQueryable<TSource> source, int page, long pageSize,
        out long recordCount)
    {
        recordCount = source.Count();
        return source.Skip((int)((page - 1) * pageSize)).Take((int)pageSize);
    }
}