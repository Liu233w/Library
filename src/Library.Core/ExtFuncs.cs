using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.LibraryService;

namespace Library
{
    public static class ExtFuncs
    {
        public static async Task<List<TR>> MapAsync<T, TR>(this IEnumerable<T> source, Func<T, Task<TR>> func)
        {
            var res = new List<TR>();
            foreach (var item in source)
            {
                res.Add(await func(item));
            }
            return res;
        }

        public static DateTime GetOutdatedTime(this BorrowRecord record)
        {
            return record.CreationTime + LibraryConsts.UserMaxBorrowDuration
                   + record.RenewTime * LibraryConsts.RenewDuration;
        }

        public static List<TR> Map<T, TR>(this IEnumerable<T> source, Func<T, TR> func)
        {
            var res = new List<TR>();
            foreach (var item in source)
            {
                res.Add(func(item));
            }
            return res;
        }
    }
}