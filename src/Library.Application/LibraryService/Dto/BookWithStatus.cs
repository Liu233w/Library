using Library.BookManage.Dto;

namespace Library.LibraryService.Dto
{
    public class BookWithStatus
    {
        public BookDto Book { get; set; }

        /// <summary>
        /// 这本书最多还能借出去多少本
        /// </summary>
        public int Avaliable { get; set; }
    }
}