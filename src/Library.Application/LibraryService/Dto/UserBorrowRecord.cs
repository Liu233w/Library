using System;
using Abp.AutoMapper;
using Library.BookManage;

namespace Library.LibraryService.Dto
{
    /// <summary>
    /// 表示当前用户的借阅记录，同一本书或同一个副本可能有多条记录
    /// </summary>
    [AutoMapFrom(typeof(Book), typeof(BookWithStatus))]
    public class UserBorrowRecord : BookWithStatus
    {
        /// <summary>
        /// 如果用户没有还书，此变量表示用户还书的 DeadLine；如果还了，此变量表示用户还书的时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 用户有没有还书
        /// </summary>
        public bool Returned { get; set; }
    }
}