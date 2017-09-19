using System;
using Abp.AutoMapper;
using Library.Users.Dto;

namespace Library.LibraryService.Dto
{
    [AutoMapFrom(typeof(BorrowRecord), typeof(BorrowRecordDto))]
    public class BorrowRecordWithAdditionalInfo : BorrowRecordDto
    {
        public string BookTitle { get; set; }

        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime BorrowTimeLimit { get; set; }

        public UserDto UserInfo { get; set; }
    }
}