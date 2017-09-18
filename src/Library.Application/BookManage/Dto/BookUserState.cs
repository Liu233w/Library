using System;
using Library.Users.Dto;

namespace Library.BookManage.Dto
{
    public class BookUserState
    {
        public UserDto User { get; set; }

        /// <summary>
        /// 应还时间
        /// </summary>
        public DateTime ReturnTime { get; set; }
    }
}