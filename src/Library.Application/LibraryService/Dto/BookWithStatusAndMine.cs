using System;
using Abp.AutoMapper;
using Library.BookManage;

namespace Library.LibraryService.Dto
{
    [AutoMapFrom(typeof(Book))]
    public class BookWithStatusAndMine : BookWithStatus
    {
        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime BorrowTimeLimit { get; set; }

        /// <summary>
        /// 用户自己有没有借这本书
        /// </summary>
        public bool Borrowed { get; set; }
    }
}