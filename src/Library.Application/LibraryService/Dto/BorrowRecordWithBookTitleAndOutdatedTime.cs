using System;
using Abp.AutoMapper;

namespace Library.LibraryService.Dto
{
    [AutoMapFrom(typeof(BorrowRecord), typeof(BorrowRecordDto))]
    public class BorrowRecordWithBookTitleAndOutdatedTime : BorrowRecordDto
    {
        public string BookTitle { get; set; }

        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime BorrowTimeLimit { get; set; }
    }
}