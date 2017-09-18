using System;

namespace Library.LibraryService.Dto
{
    public class BookWithStatusAndMine : BookWithStatus
    {
        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime BackTime { get; set; }

        /// <summary>
        /// 用户自己有没有借这本书
        /// </summary>
        public bool Borrowed { get; set; }
    }
}