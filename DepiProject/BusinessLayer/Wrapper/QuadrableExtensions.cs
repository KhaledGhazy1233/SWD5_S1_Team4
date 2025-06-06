﻿using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Wrapper;

public static class QuadrableExtensions
{
    public static async Task<PaginateResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    where T : class
    {
        if (source == null)
            throw new Exception("Empty");

        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.AsNoTracking().CountAsync();
        if (count == 0)
            return PaginateResult<T>.Success(new List<T>(), count, pageNumber, pageSize);
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return PaginateResult<T>.Success(items, pageNumber, pageSize, count);
    }
}