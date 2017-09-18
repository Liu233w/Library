using System.Collections.Generic;

namespace Library.BookManage.Dto
{
    public class BookStatusWithUserList
    {
        public BookDto Book { get; set; }

        public ICollection<BookUserState> BorrowedBooks { get; set; }
    }
}