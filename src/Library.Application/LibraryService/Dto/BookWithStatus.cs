using Abp.AutoMapper;
using Library.BookManage;
using Library.BookManage.Dto;

namespace Library.LibraryService.Dto
{
    [AutoMapFrom(typeof(Book))]
    public class BookWithStatus : BookDto
    {
        /// <summary>
        /// 这本书最多还能借出去多少本
        /// </summary>
        public int Avaliable { get; set; }
    }
}