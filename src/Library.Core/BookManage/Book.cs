using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Library.LibraryService;

namespace Library.BookManage
{
    [Table("AppBooks")]
    public class Book : Entity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Publish { get; set; }

        /// <summary>
        /// 图书馆一共有多少本书（包括借出去的）
        /// </summary>
        public int Count { get; set; }

        public ICollection<BorrowRecords> BorrowRecords { get; set; }
    }
}