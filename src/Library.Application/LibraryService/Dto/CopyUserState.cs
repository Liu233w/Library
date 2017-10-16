using System;
using Library.Users.Dto;

namespace Library.LibraryService.Dto
{
    public class CopyUserState
    {
        public UserDto User { get; set; }

        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime BorrowTimeLimit { get; set; }

        public BorrowRecordDto Record { get; set; }
    }
}