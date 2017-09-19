using System.Collections.Generic;
using Library.LibraryService.Dto;

namespace Library.LibraryService.Dto
{
    public class BookWithStatusAndRecord
    {
        public BookWithStatus Book { get; set; }

        public ICollection<BookUserState> BorrowedBooks { get; set; }
    }
}